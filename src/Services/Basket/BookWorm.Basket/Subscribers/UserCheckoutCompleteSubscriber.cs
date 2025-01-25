using BookWorm.Basket.IntegrationEvents.EventHandlers;
using BookWorm.Basket.IntegrationEvents.Events;
using Dapr;

namespace BookWorm.Basket.Subscribers;

internal sealed class UserCheckoutCompleteSubscriber
    : ISubscriber<
        OrderStatusChangedToNewIntegrationEventHandler,
        OrderStatusChangedToNewIntegrationEvent
    >
{
    public TopicOptions TopicOptions { get; set; } =
        new()
        {
            PubsubName = ServiceName.Component.Pubsub,
            Name = nameof(OrderStatusChangedToNewIntegrationEvent),
        };

    public void MapIntegrationEventEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                $"/{TopicOptions.Name}",
                async (
                    OrderStatusChangedToNewIntegrationEventHandler handler,
                    OrderStatusChangedToNewIntegrationEvent @event
                ) => await HandleAsync(handler, @event)
            )
            .WithTopic(TopicOptions)
            .ExcludeFromDescription();
    }

    public async Task HandleAsync(
        OrderStatusChangedToNewIntegrationEventHandler hander,
        OrderStatusChangedToNewIntegrationEvent @event,
        CancellationToken cancellationToken = default
    )
    {
        await hander.Handle(@event);
    }
}
