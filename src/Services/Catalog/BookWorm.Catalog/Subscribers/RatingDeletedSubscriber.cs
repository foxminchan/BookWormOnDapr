using BookWorm.Catalog.IntegrationEvents.EventHandlers;
using BookWorm.Catalog.IntegrationEvents.Events;

namespace BookWorm.Catalog.Subscribers;

internal sealed class RatingDeletedSubscriber
    : ISubscriber<RatingDeletedIntegrationEventHandler, RatingDeletedIntegrationEvent>
{
    public TopicOptions TopicOptions { get; set; } =
        new()
        {
            PubsubName = ServiceName.Component.Pubsub,
            Name = nameof(RatingDeletedIntegrationEvent),
        };

    public void MapIntegrationEventEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                $"/{TopicOptions.Name}",
                async (
                    RatingDeletedIntegrationEventHandler handler,
                    RatingDeletedIntegrationEvent @event
                ) => await HandleAsync(handler, @event)
            )
            .WithTopic(TopicOptions)
            .ExcludeFromDescription();
    }

    public async Task HandleAsync(
        RatingDeletedIntegrationEventHandler handler,
        RatingDeletedIntegrationEvent @event,
        CancellationToken cancellationToken = default
    )
    {
        await handler.Handle(@event);
    }
}
