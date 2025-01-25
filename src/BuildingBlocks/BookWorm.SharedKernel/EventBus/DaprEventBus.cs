using BookWorm.Constants;
using BookWorm.SharedKernel.EventBus.Abstractions;
using BookWorm.SharedKernel.EventBus.Events;
using Dapr.Client;

namespace BookWorm.SharedKernel.EventBus;

public sealed class DaprEventBus(DaprClient daprClient) : IEventBus
{
    private const string PubSubName = ServiceName.Component.Pubsub;

    public async Task PublishAsync(
        IntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default
    )
    {
        await daprClient.PublishEventAsync<object>(
            PubSubName,
            integrationEvent.GetType().Name,
            integrationEvent,
            cancellationToken: cancellationToken
        );
    }
}
