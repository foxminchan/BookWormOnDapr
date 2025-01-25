using BookWorm.Customer.Domain;

namespace BookWorm.Customer.Features.Get;

internal sealed class GetConsumerEndpoint
    : IEndpoint<Results<Ok<ConsumerDto>, NotFound<ProblemDetails>>, Guid, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/consumers/{id:guid}",
                async ([Description("The consumer id")] Guid id, ISender sender) =>
                    await HandleAsync(id, sender)
            )
            .Produces<ConsumerDto>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .WithTags(nameof(Consumer))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Results<Ok<ConsumerDto>, NotFound<ProblemDetails>>> HandleAsync(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(new GetConsumerQuery(id), cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new() { Detail = $"Consumer with id {id} not found." }
            )
            : TypedResults.Ok(result.Value);
    }
}
