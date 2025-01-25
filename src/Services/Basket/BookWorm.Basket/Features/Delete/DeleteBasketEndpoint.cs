namespace BookWorm.Basket.Features.Delete;

internal sealed class DeleteBasketEndpoint
    : IEndpoint<Results<NoContent, NotFound<ProblemDetails>>, Guid, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/baskets/{id:guid}",
                async ([Description("The basket id")] Guid id, ISender sender) =>
                    await HandleAsync(id, sender)
            )
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .WithTags(nameof(Basket))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Results<NoContent, NotFound<ProblemDetails>>> HandleAsync(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(new DeleteBasketCommand(id), cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new() { Detail = $"Basket with id {id} not found." }
            )
            : TypedResults.NoContent();
    }
}
