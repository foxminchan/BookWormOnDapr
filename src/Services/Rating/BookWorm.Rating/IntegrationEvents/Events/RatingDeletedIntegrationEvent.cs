namespace BookWorm.Rating.IntegrationEvents.Events;

public sealed record RatingDeletedIntegrationEvent(Guid BookId, int Rating) : IntegrationEvent;
