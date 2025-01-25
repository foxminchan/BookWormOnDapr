using BookWorm.Inventory.Domain;
using BookWorm.Inventory.Domain.Specifications;

namespace BookWorm.Inventory.Features.Stocks.Get;

internal sealed record GetStocksQuery(Guid[] ProductIds) : IQuery<Result<Dictionary<Guid, int>>>;

internal sealed class GetStocksHandler(IReadRepository<Warehouse> repository)
    : IQueryHandler<GetStocksQuery, Result<Dictionary<Guid, int>>>
{
    public async Task<Result<Dictionary<Guid, int>>> Handle(
        GetStocksQuery request,
        CancellationToken cancellationToken
    )
    {
        var warehouses = await repository.ListAsync(new WarehouseFilterSpec(), cancellationToken);

        var stocks = warehouses
            .SelectMany(warehouse => warehouse.Stocks)
            .Where(stock => request.ProductIds.Contains(stock.ProductId))
            .GroupBy(stock => stock.ProductId)
            .ToDictionary(group => group.Key, group => group.Sum(stock => stock.Quantity));

        return stocks;
    }
}
