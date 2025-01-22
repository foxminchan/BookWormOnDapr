using Ardalis.Result;
using BookWorm.Inventory.Domain;
using BookWorm.Inventory.Domain.Specifications;
using BookWorm.SharedKernel.Command;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Inventory.Features.Warehouses.Delete;

internal sealed record DeleteWarehouseCommand(long Id) : ICommand;

internal sealed class DeleteWarehouseHandler(IRepository<Warehouse> Repository)
    : ICommandHandler<DeleteWarehouseCommand>
{
    public async Task<Result> Handle(
        DeleteWarehouseCommand request,
        CancellationToken cancellationToken
    )
    {
        var warehouse = await Repository.FirstOrDefaultAsync(
            new WarehouseFilterSpec(request.Id),
            cancellationToken
        );

        if (warehouse is null)
        {
            return Result.NotFound();
        }

        warehouse.Delete();

        await Repository.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
