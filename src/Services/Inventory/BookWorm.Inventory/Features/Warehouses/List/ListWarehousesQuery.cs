using BookWorm.Inventory.Domain;
using BookWorm.Inventory.Domain.Specifications;

namespace BookWorm.Inventory.Features.Warehouses.List;

internal sealed record ListWarehousesQuery : IQuery<Result<IReadOnlyList<WarehouseDto>>>;

internal sealed class ListWarehousesHandler(IReadRepository<Warehouse> repository)
    : IQueryHandler<ListWarehousesQuery, Result<IReadOnlyList<WarehouseDto>>>
{
    public async Task<Result<IReadOnlyList<WarehouseDto>>> Handle(
        ListWarehousesQuery request,
        CancellationToken cancellationToken
    )
    {
        var warehouses = await repository.ListAsync(new WarehouseFilterSpec(), cancellationToken);

        return Result.Success(warehouses.ToWarehouseDtos());
    }
}
