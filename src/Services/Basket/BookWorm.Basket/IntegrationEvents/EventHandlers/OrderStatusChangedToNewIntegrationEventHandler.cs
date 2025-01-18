using BookWorm.Basket.Domain;
using BookWorm.Basket.IntegrationEvents.Events;
using BookWorm.SharedKernel.EventBus.Abstractions;

namespace BookWorm.Basket.IntegrationEvents.EventHandlers;

public sealed class OrderStatusChangedToNewIntegrationEventHandler(IBasketRepository repository)
    : IIntegrationEventHandler<OrderStatusChangedToNewIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToNewIntegrationEvent @event)
    {
        if (@event.CustomerId is null)
        {
            return;
        }

        await repository.DeleteAsync(@event.CustomerId);
    }
}
