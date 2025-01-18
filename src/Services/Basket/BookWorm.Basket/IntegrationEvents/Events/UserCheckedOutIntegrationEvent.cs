using BookWorm.Basket.Contracts;
using BookWorm.SharedKernel.EventBus.Events;

namespace BookWorm.Basket.IntegrationEvents.Events;

public sealed record UserCheckedOutIntegrationEvent(Guid? CustomerId, List<CardItem> Items)
    : IntegrationEvent;
