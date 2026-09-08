using MediatR;
using ProcurementSys.Application.Abstractions.Persistence;
using ProcurementSys.Domain.Entities;
using ProcurementSys.Domain.Events;

namespace ProcurementSys.Application.Features.MaterialRequests.EventHandlers;

public class MaterialRequestFullyApprovedDomainEventHandler : INotificationHandler<MaterialRequestFullyApprovedDomainEvent>
{
    private readonly IMaterialRequestRepository _materialRequestRepository;
    private readonly IProcurementRequestRepository _procurementRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MaterialRequestFullyApprovedDomainEventHandler(
        IMaterialRequestRepository materialRequestRepository,
        IProcurementRequestRepository procurementRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _materialRequestRepository = materialRequestRepository;
        _procurementRequestRepository = procurementRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(MaterialRequestFullyApprovedDomainEvent notification, CancellationToken cancellationToken)
    {
        var materialRequest = await _materialRequestRepository.GetByIdAsync(notification.MaterialRequestId, cancellationToken);
        if (materialRequest is null) return;

        var procurementNumber = $"PR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..4].ToUpper()}";

        var procurementRequest = ProcurementRequest.CreatePlaceholder(procurementNumber, materialRequest.Id);

        await _procurementRequestRepository.AddAsync(procurementRequest, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}