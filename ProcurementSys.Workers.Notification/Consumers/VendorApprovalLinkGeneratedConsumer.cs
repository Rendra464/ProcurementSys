using MassTransit;
using ProcurementSys.Contracts.IntegrationEvents;

namespace ProcurementSys.Workers.Notification.Consumers;

// Consumer khusus alur vendor non-listing: kirim email berisi magic-link ke vendor
public class VendorApprovalLinkGeneratedConsumer : IConsumer<VendorApprovalLinkGeneratedIntegrationEvent>
{
    public Task Consume(ConsumeContext<VendorApprovalLinkGeneratedIntegrationEvent> context)
    {
        // TODO: kirim email berisi context.Message.MagicLinkUrl
        return Task.CompletedTask;
    }
}
