using BookWorm.Customer.Domain;

namespace BookWorm.Customer.Features.List;

internal sealed class ListConsumersEndpoint
    : IEndpoint<Ok<PagedResult<IReadOnlyList<ConsumerDto>>>, ListConsumersQuery, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/consumers",
                async ([AsParameters] ListConsumersQuery query, ISender sender) =>
                    await HandleAsync(query, sender)
            )
            .ProducesValidationProblem()
            .Produces<PagedResult<IReadOnlyList<ConsumerDto>>>()
            .WithOpenApi()
            .WithTags(nameof(Consumer))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Ok<PagedResult<IReadOnlyList<ConsumerDto>>>> HandleAsync(
        ListConsumersQuery query,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(result);
    }
}
