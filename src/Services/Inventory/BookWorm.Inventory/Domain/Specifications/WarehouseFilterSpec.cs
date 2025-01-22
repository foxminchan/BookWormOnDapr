using Ardalis.Specification;

namespace BookWorm.Inventory.Domain.Specifications;

public sealed class WarehouseFilterSpec : Specification<Warehouse>
{
    public WarehouseFilterSpec()
    {
        Query.Where(warehouse => !warehouse.IsDeleted);
    }

    public WarehouseFilterSpec(long id)
    {
        Query.Where(warehouse => warehouse.Id == id && !warehouse.IsDeleted);
    }
}
