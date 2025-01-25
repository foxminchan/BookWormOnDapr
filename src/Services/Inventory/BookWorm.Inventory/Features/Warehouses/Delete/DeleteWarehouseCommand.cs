using BookWorm.Inventory.Domain;
using BookWorm.Inventory.Domain.Specifications;

namespace BookWorm.Inventory.Features.Warehouses.Delete;

internal sealed record DeleteWarehouseCommand(long Id) : ICommand;

internal sealed class DeleteWarehouseHandler(IRepository<Warehouse> repository)
    : ICommandHandler<DeleteWarehouseCommand>
{
    public async Task<Result> Handle(
        DeleteWarehouseCommand request,
        CancellationToken cancellationToken
    )
    {
        var warehouse = await repository.FirstOrDefaultAsync(
            new WarehouseFilterSpec(request.Id),
            cancellationToken
        );

        if (warehouse is null)
        {
            return Result.NotFound();
        }

        warehouse.Delete();

        await repository.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
