using BookWorm.Ordering.Domain.Events;

namespace BookWorm.Ordering.Domain;

public record OrderSummary(Guid Id, OrderStatus OrderStatus, decimal TotalPrice)
{
    public OrderSummary Apply(OrderPlacedEvent @event)
    {
        return this with { OrderStatus = OrderStatus.New };
    }

    public OrderSummary Apply(OrderPaidEvent @event)
    {
        return this with { OrderStatus = OrderStatus.Paid };
    }

    public OrderSummary Apply(OrderCancelledEvent @event)
    {
        return this with { OrderStatus = OrderStatus.Cancelled };
    }

    public OrderSummary Apply(OrderCompletedEvent @event)
    {
        return this with { OrderStatus = OrderStatus.Completed };
    }
}
