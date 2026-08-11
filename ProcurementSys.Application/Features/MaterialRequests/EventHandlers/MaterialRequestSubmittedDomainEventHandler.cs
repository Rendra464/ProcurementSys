using MediatR;
using ProcurementSys.Application.Abstractions.Persistence;
using ProcurementSys.Application.Common.Exceptions;
using ProcurementSys.Application.Features.Workflow.Services;
using ProcurementSys.Domain.Entities;
using ProcurementSys.Domain.Events;
using ProcurementSys.Domain.Enums;

namespace ProcurementSys.Application.Features.MaterialRequests.EventHandlers;

public class MaterialRequestSubmittedDomainEventHandler
    : INotificationHandler<MaterialRequestSubmittedDomainEvent>
{
    private readonly IMaterialRequestRepository _materialRequestRepository;
    private readonly IAssignmentApprovalOrchestrator _orchestrator;
    private readonly IUnitOfWork _unitOfWork;

    public MaterialRequestSubmittedDomainEventHandler(
        IMaterialRequestRepository materialRequestRepository,
        IAssignmentApprovalOrchestrator orchestrator,
        IUnitOfWork unitOfWork)
    {
        _materialRequestRepository = materialRequestRepository;
        _orchestrator = orchestrator;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        MaterialRequestSubmittedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var materialRequest = await _materialRequestRepository.GetByIdAsync(
            notification.MaterialRequestId,
            cancellationToken);

        if (materialRequest is null)
        {
            throw new NotFoundException(nameof(MaterialRequest), notification.MaterialRequestId);
        }

        var firstAssignment = await _orchestrator.StartApprovalAsync(
            WorkflowProcessType.MaterialRequest,
            materialRequest.Id,
            materialRequest.TotalEstimatedValue,
            cancellationToken);

        materialRequest.StartApproval();

        if (firstAssignment is null)
        {
            materialRequest.MarkFullyApproved();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}