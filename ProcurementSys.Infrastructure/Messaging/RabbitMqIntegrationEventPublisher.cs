using MassTransit;
using ProcurementSys.Application.Abstractions.Messaging;

namespace ProcurementSys.Infrastructure.Messaging;

public class RabbitMqIntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    public RabbitMqIntegrationEventPublisher(IPublishEndpoint publishEndpoint) => _publishEndpoint = publishEndpoint;

    public Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken ct) where TEvent : class
        => _publishEndpoint.Publish(integrationEvent, ct);
}
