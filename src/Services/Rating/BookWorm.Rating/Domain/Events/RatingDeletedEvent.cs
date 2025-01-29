namespace BookWorm.Rating.Domain.Events;

public class RatingDeletedEvent(Review review) : DomainEvent
{
    public Review Review { get; init; } = Guard.Against.Null(review);
}
