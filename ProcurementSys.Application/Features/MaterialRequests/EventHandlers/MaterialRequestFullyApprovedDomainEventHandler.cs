using MediatR;
using ProcurementSys.Domain.Events;

namespace ProcurementSys.Application.Features.MaterialRequests.EventHandlers;

// Domain event handler: begitu MaterialRequest fully approved, auto-create ProcurementRequest.
// Ini "Event-Driven" beneran di dalam satu proses (in-process), bukan cuma nama folder doang.
public class MaterialRequestFullyApprovedDomainEventHandler : INotificationHandler<MaterialRequestFullyApprovedDomainEvent>
{
    public Task Handle(MaterialRequestFullyApprovedDomainEvent notification, CancellationToken ct)
    {
        // TODO: panggil IProcurementRequestService.CreateFromMaterialRequestAsync(notification.MaterialRequestId, ct)
        return Task.CompletedTask;
    }
}
