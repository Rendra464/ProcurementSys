namespace ProcurementSys.Contracts.IntegrationEvents;

public record ProcurementApprovalStatusChangedIntegrationEvent(
    int ProcurementRequestId,
    string ProcurementNumber,
    string NewStatus,
    string RecipientEmail
) : IntegrationEvent;
