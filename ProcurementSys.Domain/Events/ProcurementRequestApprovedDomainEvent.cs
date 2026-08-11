using ProcurementSys.Domain.Common;

namespace ProcurementSys.Domain.Events;

public record ProcurementRequestApprovedDomainEvent(int ProcurementRequestId, string Type) : IDomainEvent;
