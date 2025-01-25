using BookWorm.Inventory.Domain;

namespace BookWorm.Inventory.Features.Warehouses.Delete;

internal sealed class DeleteWarehouseEndpoint
    : IEndpoint<Results<NoContent, NotFound<ProblemDetails>>, long, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/warehouses/{id:long}",
                async (long id, ISender sender) => await HandleAsync(id, sender)
            )
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .WithTags(nameof(Warehouse))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Results<NoContent, NotFound<ProblemDetails>>> HandleAsync(
        long id,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(new DeleteWarehouseCommand(id), cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new() { Detail = $"Warehouse with id {id} not found." }
            )
            : TypedResults.NoContent();
    }
}
