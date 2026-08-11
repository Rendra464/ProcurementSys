using ProcurementSys.Domain.Common;

namespace ProcurementSys.Domain.Events;

public record MaterialRequestRejectedDomainEvent(int MaterialRequestId) : IDomainEvent;