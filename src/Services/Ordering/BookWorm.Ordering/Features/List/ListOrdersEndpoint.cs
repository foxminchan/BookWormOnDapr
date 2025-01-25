namespace BookWorm.Ordering.Features.List;

internal sealed class ListOrdersEndpoint
    : IEndpoint<Ok<PagedResult<IReadOnlyList<OrderDto>>>, ListOrdersQuery, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/orders",
                async ([AsParameters] ListOrdersQuery query, ISender sender) =>
                    await HandleAsync(query, sender)
            )
            .ProducesValidationProblem()
            .Produces<PagedResult<IReadOnlyList<OrderDto>>>()
            .WithOpenApi()
            .WithTags(nameof(Ordering))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Ok<PagedResult<IReadOnlyList<OrderDto>>>> HandleAsync(
        ListOrdersQuery query,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(result);
    }
}
