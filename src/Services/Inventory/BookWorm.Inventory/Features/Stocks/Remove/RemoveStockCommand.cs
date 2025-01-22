using Ardalis.Result;
using BookWorm.Inventory.Domain;
using BookWorm.Inventory.Domain.Specifications;
using BookWorm.SharedKernel.Command;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Inventory.Features.Stocks.Remove;

internal sealed record RemoveStockCommand(long WarehouseId, Guid ProductId, int Quantity)
    : ICommand<Result<StockDto>>;

internal sealed class RemoveStockHandler(IRepository<Warehouse> repository)
    : ICommandHandler<RemoveStockCommand, Result<StockDto>>
{
    public async Task<Result<StockDto>> Handle(
        RemoveStockCommand request,
        CancellationToken cancellationToken
    )
    {
        var warehouse = await repository.FirstOrDefaultAsync(
            new WarehouseFilterSpec(request.WarehouseId),
            cancellationToken
        );

        var stock = warehouse?.GetStock(request.ProductId);

        if (warehouse is null || stock is null)
        {
            return Result.NotFound();
        }

        warehouse.RemoveStock(request.ProductId, request.Quantity);

        await repository.SaveChangesAsync(cancellationToken);

        return stock.ToStockDto();
    }
}
