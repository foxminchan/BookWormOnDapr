using BookWorm.Ordering.Domain;

namespace BookWorm.Ordering.Features;

public sealed record OrderDto(Guid Id, int No, OrderStatus Status, decimal TotalPrice);
