﻿namespace BookWorm.Ordering.Domain.Specifications;

public sealed class OrderFilterSpec : Specification<Order>
{
    public OrderFilterSpec(int pageIndex, int pageSize, Guid customerId)
    {
        Query
            .Where(order => !order.IsDeleted && order.ConsumerId == customerId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
    }

    public OrderFilterSpec(Guid orderId)
    {
        Query.Where(order => order.Id == orderId && !order.IsDeleted);
    }
}
