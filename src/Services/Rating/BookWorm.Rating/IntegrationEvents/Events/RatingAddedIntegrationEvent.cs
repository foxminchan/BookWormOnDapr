namespace BookWorm.Rating.IntegrationEvents.Events;

public sealed record RatingAddedIntegrationEvent(Guid BookId, int Rating) : IntegrationEvent;
