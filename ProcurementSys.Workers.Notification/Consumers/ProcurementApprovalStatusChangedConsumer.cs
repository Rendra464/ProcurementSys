using MassTransit;
using ProcurementSys.Contracts.IntegrationEvents;

namespace ProcurementSys.Workers.Notification.Consumers;

public class ProcurementApprovalStatusChangedConsumer : IConsumer<ProcurementApprovalStatusChangedIntegrationEvent>
{
    public Task Consume(ConsumeContext<ProcurementApprovalStatusChangedIntegrationEvent> context)
    {
        // TODO: kirim email (SMTP/SendGrid) + tulis log ke tabel Notifications
        return Task.CompletedTask;
    }
}
