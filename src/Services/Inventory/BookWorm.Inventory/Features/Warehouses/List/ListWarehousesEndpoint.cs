using BookWorm.Inventory.Domain;

namespace BookWorm.Inventory.Features.Warehouses.List;

internal sealed class ListWarehousesEndpoint : IEndpoint<Ok<IReadOnlyList<WarehouseDto>>, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/warehouses", async (ISender sender) => await HandleAsync(sender))
            .Produces<Ok<IReadOnlyList<WarehouseDto>>>()
            .WithOpenApi()
            .WithTags(nameof(Warehouse))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Ok<IReadOnlyList<WarehouseDto>>> HandleAsync(
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(new ListWarehousesQuery(), cancellationToken);
        return TypedResults.Ok(result.Value);
    }
}
