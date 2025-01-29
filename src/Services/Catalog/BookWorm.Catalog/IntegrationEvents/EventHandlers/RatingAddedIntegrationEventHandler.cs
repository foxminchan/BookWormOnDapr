using BookWorm.Catalog.Domain.BookAggregate;
using BookWorm.Catalog.Domain.BookAggregate.Specifications;
using BookWorm.Catalog.IntegrationEvents.Events;

namespace BookWorm.Catalog.IntegrationEvents.EventHandlers;

public sealed class RatingAddedIntegrationEventHandler(
    IRepository<Book> repository,
    ILogger<RatingAddedIntegrationEventHandler> logger
) : IIntegrationEventHandler<RatingAddedIntegrationEvent>
{
    public async Task Handle(RatingAddedIntegrationEvent @event)
    {
        logger.LogInformation(
            "[{Event}] - Handling added rating for book {BookId}",
            nameof(RatingAddedIntegrationEvent),
            @event.BookId
        );

        var book = await repository.FirstOrDefaultAsync(
            new BookFilterSpec(@event.BookId),
            CancellationToken.None
        );

        if (book is not null)
        {
            book.AddReview(@event.Rating);
            await repository.SaveChangesAsync(CancellationToken.None);
        }
    }
}
