namespace BookWorm.Rating.Features.List;

internal sealed class ListRatingsEndpoint
    : IEndpoint<Ok<PagedResult<IReadOnlyList<ReviewDto>>>, ListRatingsQuery, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/ratings",
                async ([AsParameters] ListRatingsQuery query, ISender sender) =>
                    await HandleAsync(query, sender)
            )
            .ProducesValidationProblem()
            .Produces<PagedResult<IReadOnlyList<ReviewDto>>>()
            .WithOpenApi()
            .WithTags(nameof(Rating))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Ok<PagedResult<IReadOnlyList<ReviewDto>>>> HandleAsync(
        ListRatingsQuery query,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(result);
    }
}
