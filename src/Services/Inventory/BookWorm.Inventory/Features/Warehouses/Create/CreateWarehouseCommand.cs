using BookWorm.Inventory.Domain;
using BookWorm.Inventory.Features.Stocks;

namespace BookWorm.Inventory.Features.Warehouses.Create;

internal sealed record CreateWarehouseCommand(
    string Location,
    WarehouseStatus Status,
    string Description,
    string Website,
    List<StockDto> Stocks
) : ICommand<Result<long>>;

[TxScope]
internal sealed class CreateWarehouseHandler(IRepository<Warehouse> repository)
    : ICommandHandler<CreateWarehouseCommand, Result<long>>
{
    public async Task<Result<long>> Handle(
        CreateWarehouseCommand request,
        CancellationToken cancellationToken
    )
    {
        var warehouse = new Warehouse(
            request.Location,
            request.Status,
            request.Description,
            request.Website,
            request.Stocks.Select(stock => new Stock(stock.ProductId, stock.Quantity)).ToList()
        );

        var result = await repository.AddAsync(warehouse, cancellationToken);

        return result.Id;
    }
}
