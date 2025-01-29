namespace BookWorm.Ordering.Features.Get;

internal sealed class GetOrderEndpoint
    : IEndpoint<Results<Ok<OrderDetailDto>, NotFound<ProblemDetails>>, Guid, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/orders/{id:guid}",
                async ([Description("The order id")] Guid id, ISender sender) =>
                    await HandleAsync(id, sender)
            )
            .Produces<OrderDetailDto>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .WithTags(nameof(Ordering))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Results<Ok<OrderDetailDto>, NotFound<ProblemDetails>>> HandleAsync(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(new GetOrderQuery(id), cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new() { Detail = $"Order with id {id} not found." }
            )
            : TypedResults.Ok(result.Value);
    }
}
