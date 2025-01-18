using BookWorm.Ordering.Contracts;
using BookWorm.SharedKernel.EventBus.Events;

namespace BookWorm.Ordering.IntegrationEvents.Events;

public sealed record UserCheckedOutIntegrationEvent(Guid? CustomerId, List<CardItem> Items)
    : IntegrationEvent;
