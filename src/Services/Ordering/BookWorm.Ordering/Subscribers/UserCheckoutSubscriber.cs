using BookWorm.Ordering.IntegrationEvents.EventHandlers;
using BookWorm.Ordering.IntegrationEvents.Events;
using Dapr;

namespace BookWorm.Ordering.Subscribers;

internal sealed class UserCheckoutSubscriber
    : ISubscriber<UserCheckoutIntegrationEventHandler, UserCheckedOutIntegrationEvent>
{
    public TopicOptions TopicOptions { get; set; } =
        new()
        {
            PubsubName = ServiceName.Component.Pubsub,
            Name = nameof(UserCheckedOutIntegrationEvent),
        };

    public void MapIntegrationEventEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                $"/{TopicOptions.Name}",
                async (
                    UserCheckoutIntegrationEventHandler handler,
                    UserCheckedOutIntegrationEvent @event
                ) => await HandleAsync(handler, @event)
            )
            .WithTopic(TopicOptions)
            .ExcludeFromDescription();
    }

    public async Task HandleAsync(
        UserCheckoutIntegrationEventHandler handler,
        UserCheckedOutIntegrationEvent @event,
        CancellationToken cancellationToken = default
    )
    {
        await handler.Handle(@event);
    }
}
