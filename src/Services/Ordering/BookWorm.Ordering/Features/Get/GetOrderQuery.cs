using Ardalis.Result;
using BookWorm.Ordering.Domain;
using BookWorm.Ordering.Domain.Specifications;
using BookWorm.SharedKernel.Query;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Ordering.Features.Get;

internal sealed record GetOrderQuery(Guid OrderId) : IQuery<Result<OrderDetailDto>>;

internal sealed class GetOrderHandler(IReadRepository<Order> repository)
    : IQueryHandler<GetOrderQuery, Result<OrderDetailDto>>
{
    public async Task<Result<OrderDetailDto>> Handle(
        GetOrderQuery request,
        CancellationToken cancellationToken
    )
    {
        var order = await repository.FirstOrDefaultAsync(
            new OrderFilterSpec(request.OrderId),
            cancellationToken
        );

        if (order is null)
        {
            return Result.NotFound();
        }

        return order.ToOrderDetailDto();
    }
}
