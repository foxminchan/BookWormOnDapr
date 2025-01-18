using BookWorm.Basket.IntegrationEvents.EventHandlers;
using BookWorm.Basket.IntegrationEvents.Events;
using BookWorm.Constants;
using BookWorm.SharedKernel.Endpoints;
using Dapr;

namespace BookWorm.Basket.Subscribers;

internal sealed class UserCheckoutCompleteSubscriber
    : ISubscriber<
        OrderStatusChangedToNewIntegrationEventHandler,
        OrderStatusChangedToNewIntegrationEvent
    >
{
    public TopicOptions topicOptions { get; set; } =
        new TopicOptions
        {
            PubsubName = ServiceName.Component.Pubsub,
            Name = nameof(OrderStatusChangedToNewIntegrationEvent),
        };

    public void MapIntegrationEventEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                $"/{topicOptions.Name}",
                async (
                    OrderStatusChangedToNewIntegrationEventHandler handler,
                    OrderStatusChangedToNewIntegrationEvent @event
                ) => await HandleAsync(handler, @event)
            )
            .WithTopic(topicOptions)
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
