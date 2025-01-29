using BookWorm.Catalog.IntegrationEvents.EventHandlers;
using BookWorm.Catalog.IntegrationEvents.Events;

namespace BookWorm.Catalog.Subscribers;

internal sealed class RatingAddedSubscriber
    : ISubscriber<RatingAddedIntegrationEventHandler, RatingAddedIntegrationEvent>
{
    public TopicOptions TopicOptions { get; set; } =
        new()
        {
            PubsubName = ServiceName.Component.Pubsub,
            Name = nameof(RatingAddedIntegrationEvent),
        };

    public void MapIntegrationEventEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                $"/{TopicOptions.Name}",
                async (
                    RatingAddedIntegrationEventHandler handler,
                    RatingAddedIntegrationEvent @event
                ) => await HandleAsync(handler, @event)
            )
            .WithTopic(TopicOptions)
            .ExcludeFromDescription();
    }

    public async Task HandleAsync(
        RatingAddedIntegrationEventHandler handler,
        RatingAddedIntegrationEvent @event,
        CancellationToken cancellationToken = default
    )
    {
        await handler.Handle(@event);
    }
}
