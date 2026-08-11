using ProcurementSys.Domain.Common;

namespace ProcurementSys.Domain.Events;

public record MaterialRequestSubmittedDomainEvent(int MaterialRequestId) : IDomainEvent;
