using ProcurementSys.Contracts.IntegrationEvents;

namespace ProcurementSys.Application.Abstractions.Messaging;
public interface IIntegrationEventPublisher
{
    Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken = default)
        where TEvent : IntegrationEvent;
}