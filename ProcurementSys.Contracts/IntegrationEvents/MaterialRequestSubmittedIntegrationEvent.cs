namespace ProcurementSys.Contracts.IntegrationEvents;

public record MaterialRequestSubmittedIntegrationEvent(
    int MaterialRequestId,
    string RequestNumber,
    string ApproverEmail
) : IntegrationEvent;
