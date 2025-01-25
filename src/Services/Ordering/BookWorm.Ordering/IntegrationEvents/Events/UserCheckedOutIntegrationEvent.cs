using BookWorm.Ordering.Contracts;

namespace BookWorm.Ordering.IntegrationEvents.Events;

public sealed record UserCheckedOutIntegrationEvent(Guid? CustomerId, List<CardItem> Items)
    : IntegrationEvent;
