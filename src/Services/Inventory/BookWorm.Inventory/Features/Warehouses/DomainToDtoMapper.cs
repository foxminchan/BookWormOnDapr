using BookWorm.Inventory.Domain;
using BookWorm.Inventory.Features.Stocks;

namespace BookWorm.Inventory.Features.Warehouses;

public static class DomainToDtoMapper
{
    public static WarehouseDto ToWarehouseDto(this Warehouse warehouse)
    {
        return new WarehouseDto(
            warehouse.Id,
            warehouse.Location,
            warehouse.Status,
            warehouse.Description,
            warehouse.Website
        );
    }

    public static IReadOnlyList<WarehouseDto> ToWarehouseDtos(this List<Warehouse> warehouses)
    {
        return warehouses.Select(warehouse => warehouse.ToWarehouseDto()).ToList();
    }

    public static WareouseDetailDto ToWarehouseDetailDto(this Warehouse warehouse)
    {
        return new WareouseDetailDto(
            warehouse.Id,
            warehouse.Location,
            warehouse.Status,
            warehouse.Description,
            warehouse.Website,
            warehouse.Stocks.ToStockDtos()
        );
    }
}
