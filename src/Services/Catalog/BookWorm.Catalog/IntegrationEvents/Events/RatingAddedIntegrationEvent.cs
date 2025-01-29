namespace BookWorm.Catalog.IntegrationEvents.Events;

public sealed record RatingAddedIntegrationEvent(Guid BookId, int Rating) : IntegrationEvent;
