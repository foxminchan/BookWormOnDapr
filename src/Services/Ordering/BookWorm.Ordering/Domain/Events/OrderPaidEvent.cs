using Ardalis.GuardClauses;
using BookWorm.SharedKernel.Core.Event;

namespace BookWorm.Ordering.Domain.Events;

public sealed class OrderPaidEvent(Guid id, decimal totalPrice) : DomainEvent
{
    public Guid Id { get; init; } = Guard.Against.Default(id);
    public OrderStatus Status { get; init; } = OrderStatus.New;
    public decimal TotalPrice { get; init; } = Guard.Against.Negative(totalPrice);
}
