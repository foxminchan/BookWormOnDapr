using BookWorm.Catalog.Domain.BookAggregate;
using BookWorm.Catalog.Domain.BookAggregate.Specifications;
using BookWorm.Catalog.IntegrationEvents.Events;

namespace BookWorm.Catalog.IntegrationEvents.EventHandlers;

public sealed class RatingDeletedIntegrationEventHandler(
    IRepository<Book> repository,
    ILogger<RatingDeletedIntegrationEventHandler> logger
) : IIntegrationEventHandler<RatingDeletedIntegrationEvent>
{
    public async Task Handle(RatingDeletedIntegrationEvent @event)
    {
        logger.LogInformation(
            "[{Event}] - Handling deleted rating for book {BookId}",
            nameof(RatingDeletedIntegrationEvent),
            @event.BookId
        );

        var book = await repository.FirstOrDefaultAsync(
            new BookFilterSpec(@event.BookId),
            CancellationToken.None
        );

        if (book is not null)
        {
            book.RemoveReview(@event.Rating);
            await repository.SaveChangesAsync(CancellationToken.None);
        }
    }
}
