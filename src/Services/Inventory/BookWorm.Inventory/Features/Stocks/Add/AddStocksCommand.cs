using BookWorm.Inventory.Domain;
using BookWorm.Inventory.Domain.Specifications;

namespace BookWorm.Inventory.Features.Stocks.Add;

internal sealed record AddStocksCommand(long WarehouseId, List<StockDto> Items) : ICommand;

internal sealed class AddStocksHandler(IRepository<Warehouse> repository)
    : ICommandHandler<AddStocksCommand>
{
    public async Task<Result> Handle(AddStocksCommand request, CancellationToken cancellationToken)
    {
        var warehouse = await repository.FirstOrDefaultAsync(
            new WarehouseFilterSpec(request.WarehouseId),
            cancellationToken
        );

        if (warehouse is null)
        {
            return Result.NotFound();
        }

        warehouse.AddStocks(
            request.Items.Select(stock => new Stock(stock.ProductId, stock.Quantity)).ToList()
        );

        await repository.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
