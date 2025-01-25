using BookWorm.Inventory.Domain;
using BookWorm.Inventory.Domain.Specifications;

namespace BookWorm.Inventory.Features.Warehouses.Get;

internal sealed record GetWarehouseQuery(long Id) : IQuery<Result<WarehouseDetailDto>>;

internal sealed class GetWarehouseHandler(IReadRepository<Warehouse> repository)
    : IQueryHandler<GetWarehouseQuery, Result<WarehouseDetailDto>>
{
    public async Task<Result<WarehouseDetailDto>> Handle(
        GetWarehouseQuery request,
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

        return warehouse.ToWarehouseDetailDto();
    }
}
