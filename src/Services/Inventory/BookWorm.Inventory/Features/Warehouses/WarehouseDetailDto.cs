using BookWorm.Inventory.Domain;
using BookWorm.Inventory.Features.Stocks;

namespace BookWorm.Inventory.Features.Warehouses;

public sealed record WarehouseDetailDto(
    long Id,
    string? Location,
    WarehouseStatus Status,
    string? Description,
    string? Website,
    IReadOnlyList<StockDto> Stocks
);
