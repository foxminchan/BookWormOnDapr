using BookWorm.Rating.Domain.Events;
using BookWorm.Rating.IntegrationEvents.Events;

namespace BookWorm.Rating.Domain.EventHandlers;

public sealed class RatingCreatedEventHandler(
    IEventBus eventBus,
    ILogger<RatingCreatedEventHandler> logger
) : INotificationHandler<RatingCreatedEvent>
{
    public async Task Handle(RatingCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[{Event}] - Handling for {BookId}",
            nameof(RatingCreatedEvent),
            notification.Review.BookId
        );

        await eventBus.PublishAsync(
            new RatingAddedIntegrationEvent(notification.Review.BookId, notification.Review.Rating),
            cancellationToken
        );
    }
}
