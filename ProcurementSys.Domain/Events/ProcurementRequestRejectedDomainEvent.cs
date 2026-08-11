using ProcurementSys.Domain.Common;

namespace ProcurementSys.Domain.Events;

public record ProcurementRequestRejectedDomainEvent(int ProcurementRequestId) : IDomainEvent;