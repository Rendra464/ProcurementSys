using MediatR;
using ProcurementSys.Application.Abstractions.Persistence;
using ProcurementSys.Application.Abstractions.Services;
using ProcurementSys.Application.Common.Exceptions;
using ProcurementSys.Domain.Entities;
using ProcurementSys.Domain.Enums;

namespace ProcurementSys.Application.Features.Workflow;

public record RejectAssignmentCommand(int AssignmentId, string? Notes = null) : IRequest;

public class RejectAssignmentCommandHandler : IRequestHandler<RejectAssignmentCommand>
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IApprovalHistoryRepository _approvalHistoryRepository;
    private readonly IMaterialRequestRepository _materialRequestRepository;
    private readonly IProcurementRequestRepository _procurementRequestRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public RejectAssignmentCommandHandler(
        IAssignmentRepository assignmentRepository,
        IApprovalHistoryRepository approvalHistoryRepository,
        IMaterialRequestRepository materialRequestRepository,
        IProcurementRequestRepository procurementRequestRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _assignmentRepository = assignmentRepository;
        _approvalHistoryRepository = approvalHistoryRepository;
        _materialRequestRepository = materialRequestRepository;
        _procurementRequestRepository = procurementRequestRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RejectAssignmentCommand request, CancellationToken cancellationToken)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(request.AssignmentId, cancellationToken);
        if (assignment is null)
        {
            throw new NotFoundException(nameof(Assignment), request.AssignmentId);
        }

        int currentUserId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("Pengguna belum terautentikasi.");

        assignment.Reject(currentUserId, request.Notes);

        var history = ApprovalHistory.Create(assignment.Id, ApprovalAction.Rejected, currentUserId, request.Notes);
        await _approvalHistoryRepository.AddAsync(history, cancellationToken);

        if (assignment.EntityType == WorkflowProcessType.MaterialRequest)
        {
            var mr = await _materialRequestRepository.GetByIdAsync(assignment.EntityId, cancellationToken);
            if (mr is null)
            {
                throw new NotFoundException(nameof(MaterialRequest), assignment.EntityId);
            }
            mr.MarkRejected();
        }
        else
        {
            var pr = await _procurementRequestRepository.GetByIdAsync(assignment.EntityId, cancellationToken);
            if (pr is null)
            {
                throw new NotFoundException(nameof(ProcurementRequest), assignment.EntityId);
            }
            pr.MarkRejected();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}