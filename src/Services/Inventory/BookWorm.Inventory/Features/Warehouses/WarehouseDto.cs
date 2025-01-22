using BookWorm.Inventory.Domain;

namespace BookWorm.Inventory.Features.Warehouses;

public sealed record WarehouseDto(
    long Id,
    string? Location,
    WarehouseStatus Status,
    string? Description,
    string? Website
);
