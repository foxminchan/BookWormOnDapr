using BookWorm.Basket.Domain;

namespace BookWorm.Basket.Features.Get;

internal sealed class GetBasketEndpoint
    : IEndpoint<Results<Ok<Card>, NotFound<ProblemDetails>>, Guid, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/baskets/{id:guid}",
                async ([Description("The basket id")] Guid id, ISender sender) =>
                    await HandleAsync(id, sender)
            )
            .Produces<Card>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .WithTags(nameof(Basket))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Results<Ok<Card>, NotFound<ProblemDetails>>> HandleAsync(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(new GetBasketQuery(id), cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new() { Detail = $"Basket with id {id} not found." }
            )
            : TypedResults.Ok(result.Value);
    }
}
