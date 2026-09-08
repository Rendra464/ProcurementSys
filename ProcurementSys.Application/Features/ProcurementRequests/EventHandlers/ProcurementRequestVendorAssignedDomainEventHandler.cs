using MediatR;
using ProcurementSys.Application.Abstractions.Persistence;
using ProcurementSys.Application.Common.Exceptions;
using ProcurementSys.Application.Features.Workflow.Services;
using ProcurementSys.Domain.Entities;
using ProcurementSys.Domain.Enums;
using ProcurementSys.Domain.Events;

namespace ProcurementSys.Application.Features.ProcurementRequests.EventHandlers;

public class ProcurementRequestVendorAssignedDomainEventHandler : INotificationHandler<ProcurementRequestVendorAssignedDomainEvent>
{
    private readonly IProcurementRequestRepository _procurementRequestRepository;
    private readonly IMaterialRequestRepository _materialRequestRepository;
    private readonly IVendorRepository _vendorRepository;
    private readonly IVendorApprovalTokenRepository _vendorApprovalTokenRepository;
    private readonly IAssignmentApprovalOrchestrator _orchestrator;
    private readonly IUnitOfWork _unitOfWork;

    public ProcurementRequestVendorAssignedDomainEventHandler(
        IProcurementRequestRepository procurementRequestRepository,
        IMaterialRequestRepository materialRequestRepository,
        IVendorRepository vendorRepository,
        IVendorApprovalTokenRepository vendorApprovalTokenRepository,
        IAssignmentApprovalOrchestrator orchestrator,
        IUnitOfWork unitOfWork)
    {
        _procurementRequestRepository = procurementRequestRepository;
        _materialRequestRepository = materialRequestRepository;
        _vendorRepository = vendorRepository;
        _vendorApprovalTokenRepository = vendorApprovalTokenRepository;
        _orchestrator = orchestrator;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ProcurementRequestVendorAssignedDomainEvent notification, CancellationToken cancellationToken)
    {
        var procurementRequest = await _procurementRequestRepository.GetByIdAsync(notification.ProcurementRequestId, cancellationToken)
            ?? throw new NotFoundException(nameof(ProcurementRequest), notification.ProcurementRequestId);

        var materialRequest = await _materialRequestRepository.GetByIdAsync(procurementRequest.MaterialRequestId, cancellationToken)
            ?? throw new NotFoundException(nameof(MaterialRequest), procurementRequest.MaterialRequestId);

        var workflowType = procurementRequest.Type == ProcurementRequestType.Listing
            ? WorkflowProcessType.ProcurementListing
            : WorkflowProcessType.ProcurementNonListing;

        decimal entityValue = materialRequest.TotalEstimatedValue;

        var firstAssignment = await _orchestrator.StartApprovalAsync(
            workflowType,
            procurementRequest.Id,
            entityValue,
            cancellationToken);

        procurementRequest.StartApproval();

        if (firstAssignment is null)
        {
            procurementRequest.MarkFullyApproved();
        }
        else
        {
            if (firstAssignment.AssigneeUserId is null &&
                procurementRequest.Type == ProcurementRequestType.NonListing &&
                procurementRequest.VendorId.HasValue)
            {
                var vendor = await _vendorRepository.GetByIdAsync(procurementRequest.VendorId.Value, cancellationToken)
                    ?? throw new NotFoundException(nameof(Vendor), procurementRequest.VendorId.Value);

                var token = VendorApprovalToken.Generate(
                    procurementRequest.Id,
                    procurementRequest.VendorId.Value,
                    firstAssignment.Id,
                    vendor.Email,
                    TimeSpan.FromDays(3));

                await _vendorApprovalTokenRepository.AddAsync(token, cancellationToken);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}