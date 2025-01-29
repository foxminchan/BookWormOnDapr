namespace BookWorm.Ordering.Features.List;

internal sealed class ListOrdersEndpoint
    : IEndpoint<Ok<PagedResult<IReadOnlyList<OrderDto>>>, ListOrdersQuery, ClaimsPrincipal, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/orders",
                async (
                    [AsParameters] ListOrdersQuery query,
                    ClaimsPrincipal claimsPrincipal,
                    ISender sender
                ) => await HandleAsync(query, claimsPrincipal, sender)
            )
            .ProducesValidationProblem()
            .Produces<PagedResult<IReadOnlyList<OrderDto>>>()
            .WithOpenApi()
            .WithTags(nameof(Ordering))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Ok<PagedResult<IReadOnlyList<OrderDto>>>> HandleAsync(
        ListOrdersQuery query,
        ClaimsPrincipal claimsPrincipal,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var customerId = Guid.Parse(claimsPrincipal.GetCustomerId());

        var result = await sender.Send(query with { CustomerId = customerId }, cancellationToken);

        return TypedResults.Ok(result);
    }
}
