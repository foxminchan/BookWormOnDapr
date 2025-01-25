using BookWorm.Inventory.Domain;

namespace BookWorm.Inventory.Features.Warehouses.Get;

internal sealed class GetWarehouseEndpoint
    : IEndpoint<Results<Ok<WarehouseDetailDto>, NotFound<ProblemDetails>>, long, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/warehouses/{id:long}",
                async ([Description("The warehouse id")] long id, ISender sender) =>
                    await HandleAsync(id, sender)
            )
            .Produces<WarehouseDetailDto>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .WithTags(nameof(Warehouse))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Results<Ok<WarehouseDetailDto>, NotFound<ProblemDetails>>> HandleAsync(
        long id,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(new GetWarehouseQuery(id), cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new() { Detail = $"Warehouse with id {id} not found." }
            )
            : TypedResults.Ok(result.Value);
    }
}
