namespace BookWorm.Rating.Domain.Events;

public sealed class RatingCreatedEvent(Review review) : DomainEvent
{
    public Review Review { get; init; } = Guard.Against.Null(review);
}
