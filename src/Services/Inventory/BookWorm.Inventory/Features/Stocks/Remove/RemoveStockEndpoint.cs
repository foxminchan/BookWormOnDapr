using BookWorm.Inventory.Domain;

namespace BookWorm.Inventory.Features.Stocks.Remove;

internal sealed class RemoveStockEndpoint
    : IEndpoint<Results<Ok<StockDto>, NotFound<ProblemDetails>>, RemoveStockCommand, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/warehouses/{warehouseId:long}/stocks/{productId:guid}",
                async (
                    [Description("The warehouse id")] long warehouseId,
                    [Description("The product id")] Guid productId,
                    [Description("The quantity to remove")] int quantity,
                    ISender sender
                ) => await HandleAsync(new(warehouseId, productId, quantity), sender)
            )
            .Produces<StockDto>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .WithOpenApi()
            .WithTags(nameof(Stock))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Results<Ok<StockDto>, NotFound<ProblemDetails>>> HandleAsync(
        RemoveStockCommand command,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(command, cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new()
                {
                    Detail =
                        $"Warehouse with id {command.WarehouseId} or stock with product id {command.ProductId} not found.",
                }
            )
            : TypedResults.Ok(result.Value);
    }
}
