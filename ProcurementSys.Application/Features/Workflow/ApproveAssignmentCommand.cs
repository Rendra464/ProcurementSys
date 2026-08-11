using MediatR;
using ProcurementSys.Application.Abstractions.Persistence;
using ProcurementSys.Application.Abstractions.Services;
using ProcurementSys.Application.Common.Exceptions;
using ProcurementSys.Application.Features.Workflow.Services;
using ProcurementSys.Domain.Entities;
using ProcurementSys.Domain.Enums;

namespace ProcurementSys.Application.Features.Workflow;

public record ApproveAssignmentCommand(int AssignmentId, string? Notes = null) : IRequest;

public class ApproveAssignmentCommandHandler : IRequestHandler<ApproveAssignmentCommand>
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IApprovalHistoryRepository _approvalHistoryRepository;
    private readonly IMaterialRequestRepository _materialRequestRepository;
    private readonly IProcurementRequestRepository _procurementRequestRepository;
    private readonly IAssignmentApprovalOrchestrator _orchestrator;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveAssignmentCommandHandler(
        IAssignmentRepository assignmentRepository,
        IApprovalHistoryRepository approvalHistoryRepository,
        IMaterialRequestRepository materialRequestRepository,
        IProcurementRequestRepository procurementRequestRepository,
        IAssignmentApprovalOrchestrator orchestrator,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _assignmentRepository = assignmentRepository;
        _approvalHistoryRepository = approvalHistoryRepository;
        _materialRequestRepository = materialRequestRepository;
        _procurementRequestRepository = procurementRequestRepository;
        _orchestrator = orchestrator;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ApproveAssignmentCommand request, CancellationToken cancellationToken)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(request.AssignmentId, cancellationToken);
        if (assignment is null)
        {
            throw new NotFoundException(nameof(Assignment), request.AssignmentId);
        }

        int currentUserId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("Pengguna belum terautentikasi.");

        assignment.Approve(currentUserId, request.Notes);

        var history = ApprovalHistory.Create(assignment.Id, ApprovalAction.Approved, currentUserId, request.Notes);
        await _approvalHistoryRepository.AddAsync(history, cancellationToken);

        decimal entityValue = 0;
        MaterialRequest? materialRequest = null;
        ProcurementRequest? procurementRequest = null;

        if (assignment.EntityType == WorkflowProcessType.MaterialRequest)
        {
            materialRequest = await _materialRequestRepository.GetByIdAsync(assignment.EntityId, cancellationToken);
            if (materialRequest is null)
            {
                throw new NotFoundException(nameof(MaterialRequest), assignment.EntityId);
            }
            entityValue = materialRequest.TotalEstimatedValue;
        }
        else
        {
            procurementRequest = await _procurementRequestRepository.GetByIdAsync(assignment.EntityId, cancellationToken);
            if (procurementRequest is null)
            {
                throw new NotFoundException(nameof(ProcurementRequest), assignment.EntityId);
            }
            var parentMr = await _materialRequestRepository.GetByIdAsync(procurementRequest.MaterialRequestId, cancellationToken);
            entityValue = parentMr?.TotalEstimatedValue ?? 0;
        }

        var nextAssignment = await _orchestrator.AdvanceApprovalAsync(assignment, entityValue, cancellationToken);

        if (nextAssignment is null)
        {
            if (materialRequest is not null)
            {
                materialRequest.MarkFullyApproved();
            }
            else if (procurementRequest is not null)
            {
                procurementRequest.MarkFullyApproved();
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}