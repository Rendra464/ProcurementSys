namespace ProcurementSys.Contracts.IntegrationEvents;

// Dipublish saat ProcurementRequest non-listing dibuat -> NotificationWorker kirim email berisi magic-link ke vendor
public record VendorApprovalLinkGeneratedIntegrationEvent(
    int ProcurementRequestId,
    string VendorEmail,
    string MagicLinkUrl,
    DateTime ExpiredAt
) : IntegrationEvent;
