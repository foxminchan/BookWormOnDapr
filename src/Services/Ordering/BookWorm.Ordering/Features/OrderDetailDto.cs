using BookWorm.Ordering.Domain;

namespace BookWorm.Ordering.Features;

public sealed record OrderDetailDto(
    Guid OrderId,
    int No,
    Guid? ConsumerId,
    OrderStatus Status,
    decimal Total,
    IReadOnlyList<LineItemDto> Items
);
