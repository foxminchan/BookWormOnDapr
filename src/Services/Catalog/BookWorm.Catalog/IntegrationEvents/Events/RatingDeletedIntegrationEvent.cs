namespace BookWorm.Catalog.IntegrationEvents.Events;

public sealed record RatingDeletedIntegrationEvent(Guid BookId, int Rating) : IntegrationEvent;
