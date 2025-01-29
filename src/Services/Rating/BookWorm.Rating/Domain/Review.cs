using BookWorm.Rating.Domain.Events;

namespace BookWorm.Rating.Domain;

public sealed class Review() : AuditableEntity, IAggregateRoot
{
    public int Rating { get; private set; }
    public string? Comment { get; private set; }
    public Guid BookId { get; private set; }
    public Guid CustomerId { get; private set; }

    public Review(int rating, string? comment, Guid bookId, Guid customerId)
        : this()
    {
        Rating = Guard.Against.OutOfRange(rating, nameof(rating), 1, 5);
        Comment = comment;
        BookId = Guard.Against.Default(bookId);
        CustomerId = Guard.Against.Default(customerId);
        RegisterDomainEvent(new RatingCreatedEvent(this));
    }

    public void Delete()
    {
        RegisterDomainEvent(new RatingDeletedEvent(this));
    }
}
