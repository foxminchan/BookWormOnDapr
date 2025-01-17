using BookWor.Constants;
using BookWorm.SharedKernel.EventBus.Abstractions;
using BookWorm.SharedKernel.EventBus.Events;
using Dapr.Client;

namespace BookWorm.SharedKernel.EventBus;

public sealed class DaprEventBus(DaprClient daprClient) : IEventBus
{
    private const string PubSubName = ServiceName.Component.Pubsub;

    public async Task PublishAsync(
        IntegrationEvent integrationEvent,
        CancellationToken cancellationToken
    )
    {
        await daprClient.PublishEventAsync(
            PubSubName,
            integrationEvent.GetType().Name,
            (object)integrationEvent,
            cancellationToken: cancellationToken
        );
    }
}
