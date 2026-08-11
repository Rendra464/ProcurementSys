using ProcurementSys.Domain.Common;

namespace ProcurementSys.Domain.Events;

// In-process event, di-handle di Application/Features/MaterialRequests/EventHandlers
// buat auto-create ProcurementRequest.
public record MaterialRequestFullyApprovedDomainEvent(int MaterialRequestId) : IDomainEvent;
