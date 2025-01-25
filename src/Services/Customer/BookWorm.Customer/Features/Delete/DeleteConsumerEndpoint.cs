using BookWorm.Customer.Domain;

namespace BookWorm.Customer.Features.Delete;

internal sealed class DeleteConsumerEndpoint
    : IEndpoint<Results<NoContent, NotFound<ProblemDetails>>, Guid, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/consumers/{id:guid}",
                async ([Description("The consumer id")] Guid id, ISender sender) =>
                    await HandleAsync(id, sender)
            )
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .WithTags(nameof(Consumer))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Results<NoContent, NotFound<ProblemDetails>>> HandleAsync(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(new DeleteConsumerCommand(id), cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new() { Detail = $"Consumer with id {id} not found." }
            )
            : TypedResults.NoContent();
    }
}
