namespace BookWorm.Basket.IntegrationEvents.Events;

public sealed record UserCheckedOutIntegrationEvent(Guid? CustomerId, List<CardItem> Items)
    : IntegrationEvent;
