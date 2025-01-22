using Ardalis.Result;
using BookWorm.Inventory.Domain;
using BookWorm.Inventory.Features.Stocks;
using BookWorm.Shared.EF;
using BookWorm.SharedKernel.Command;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Inventory.Features.Warehouses.Create;

internal sealed record CreateWarehouseCommand(
    string Location,
    WarehouseStatus Status,
    string Description,
    string Website,
    List<StockDto> Stocks
) : ICommand<Result<long>>;

[TxScope]
internal sealed record CreateWarehouseHandler(IRepository<Warehouse> Repository)
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

        var result = await Repository.AddAsync(warehouse, cancellationToken);

        return result.Id;
    }
}
