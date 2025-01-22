using Ardalis.Result;
using BookWorm.Inventory.Domain;
using BookWorm.Inventory.Domain.Specifications;
using BookWorm.SharedKernel.Query;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Inventory.Features.Warehouses.Get;

internal sealed record GetWarehouseQuery(long Id) : IQuery<Result<WareouseDetailDto>>;

internal sealed class GetWarehouseHandler(IReadRepository<Warehouse> repository)
    : IQueryHandler<GetWarehouseQuery, Result<WareouseDetailDto>>
{
    public async Task<Result<WareouseDetailDto>> Handle(
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
