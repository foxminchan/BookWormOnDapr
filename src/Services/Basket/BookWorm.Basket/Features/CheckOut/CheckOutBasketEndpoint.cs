namespace BookWorm.Basket.Features.CheckOut;

internal sealed class CheckOutBasketEndpoint
    : IEndpoint<Results<Accepted, NotFound<ProblemDetails>>, Guid, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/baskets/{id:guid}/checkout",
                async ([Description("The customer id")] Guid id, ISender sender) =>
                    await HandleAsync(id, sender)
            )
            .Produces(StatusCodes.Status202Accepted)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .WithTags(nameof(Basket))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Results<Accepted, NotFound<ProblemDetails>>> HandleAsync(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(new CheckOutBasketCommand(id), cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new() { Detail = $"Basket with id {id} not found." }
            )
            : TypedResults.Accepted(
                new UrlBuilder().WithVersion().WithResource("Baskets").WithId(result.Value).Build()
            );
    }
}
