using ProcurementSys.Domain.Common;

namespace ProcurementSys.Domain.Events;

public record ProcurementRequestVendorAssignedDomainEvent(int ProcurementRequestId) : IDomainEvent;