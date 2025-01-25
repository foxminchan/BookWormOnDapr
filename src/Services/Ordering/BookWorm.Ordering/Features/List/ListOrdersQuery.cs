using BookWorm.Ordering.Domain;
using BookWorm.Ordering.Domain.Specifications;

namespace BookWorm.Ordering.Features.List;

internal sealed record ListOrdersQuery(
    [property: Description("Number of items to return in a single page of results")]
    [property: DefaultValue(Pagination.DefaultPageIndex)]
        int PageIndex = Pagination.DefaultPageIndex,
    [property: Description("Number of items to return in a single page of results")]
    [property: DefaultValue(Pagination.DefaultPageSize)]
        int PageSize = Pagination.DefaultPageSize
) : IQuery<PagedResult<IReadOnlyList<OrderDto>>>;

internal sealed class ListOrdersHandler(IReadRepository<Order> repository)
    : IQueryHandler<ListOrdersQuery, PagedResult<IReadOnlyList<OrderDto>>>
{
    public async Task<PagedResult<IReadOnlyList<OrderDto>>> Handle(
        ListOrdersQuery request,
        CancellationToken cancellationToken
    )
    {
        var orders = repository.ListAsync(
            new OrderFilterSpec(request.PageIndex, request.PageSize),
            cancellationToken
        );

        var totalRecords = repository.CountAsync(cancellationToken);

        await Task.WhenAll(orders, totalRecords);

        var totalPages = (int)Math.Ceiling(totalRecords.Result / (double)request.PageSize);

        PagedInfo pagedInfo = new(
            request.PageIndex,
            request.PageSize,
            totalRecords.Result,
            totalPages
        );

        return new(pagedInfo, orders.Result.ToOrderDtos());
    }
}
