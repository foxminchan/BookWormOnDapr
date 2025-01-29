using BookWorm.Rating.Domain.Events;
using BookWorm.Rating.IntegrationEvents.Events;

namespace BookWorm.Rating.Domain.EventHandlers;

public sealed class RatingDeletedEventHandler(
    IEventBus eventBus,
    ILogger<RatingDeletedEventHandler> logger
) : INotificationHandler<RatingDeletedEvent>
{
    public async Task Handle(RatingDeletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[{Event}] - Handling for {BookId}",
            nameof(RatingDeletedEvent),
            notification.Review.BookId
        );
        await eventBus.PublishAsync(
            new RatingDeletedIntegrationEvent(
                notification.Review.BookId,
                notification.Review.Rating
            ),
            cancellationToken
        );
    }
}
