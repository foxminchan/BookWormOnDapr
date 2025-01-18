using BookWorm.Constants;
using BookWorm.Ordering.IntegrationEvents.EventHandlers;
using BookWorm.Ordering.IntegrationEvents.Events;
using BookWorm.SharedKernel.Endpoints;
using Dapr;

namespace BookWorm.Ordering.Subscribers;

internal sealed class UserCheckoutSubscriber
    : ISubscriber<UserCheckoutIntegrationEventHandler, UserCheckedOutIntegrationEvent>
{
    public TopicOptions topicOptions { get; set; } =
        new TopicOptions
        {
            PubsubName = ServiceName.Component.Pubsub,
            Name = nameof(UserCheckedOutIntegrationEvent),
        };

    public void MapIntegrationEventEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                $"/{topicOptions.Name}",
                async (
                    UserCheckoutIntegrationEventHandler handler,
                    UserCheckedOutIntegrationEvent @event
                ) => await HandleAsync(handler, @event)
            )
            .WithTopic(topicOptions)
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
