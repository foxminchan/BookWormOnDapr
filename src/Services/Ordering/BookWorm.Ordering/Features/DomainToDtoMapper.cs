using BookWorm.Ordering.Domain;

namespace BookWorm.Ordering.Features;

public static class DomainToDtoMapper
{
    public static OrderDetailDto ToOrderDetailDto(this Order order)
    {
        return new(
            order.Id,
            order.No,
            order.ConsumerId,
            order.Status,
            order.Items.Sum(item => item.Price * item.Quantity),
            order.Items.ToLineItemDtos()
        );
    }

    public static IReadOnlyList<LineItemDto> ToLineItemDtos(this IReadOnlyCollection<Item> items)
    {
        return items.Select(item => new LineItemDto(item.Id, item.Quantity, item.Price)).ToList();
    }

    public static OrderDto ToOrderDto(this Order order)
    {
        return new(
            order.Id,
            order.No,
            order.Status,
            order.Items.Sum(item => item.Price * item.Quantity)
        );
    }

    public static IReadOnlyList<OrderDto> ToOrderDtos(this List<Order> orders)
    {
        return orders.Select(order => order.ToOrderDto()).ToList();
    }
}
