using BookWorm.SharedKernel.EventBus.Events;

namespace BookWorm.Basket.IntegrationEvents.Events;

public sealed record OrderStatusChangedToNewIntegrationEvent(
    Guid OrderId,
    string OrderStatus,
    Guid? CustomerId
) : IntegrationEvent;
