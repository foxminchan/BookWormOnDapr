using BookWorm.Catalog.Domain;

namespace BookWorm.Catalog.Features.Categories.Delete;

internal sealed class DeleteCategoryEndpoint
    : IEndpoint<Results<NoContent, NotFound<ProblemDetails>>, Guid, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/categories/{id:guid}",
                async ([Description("The category id")] Guid id, ISender sender) =>
                    await HandleAsync(id, sender)
            )
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .WithTags(nameof(Category))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Results<NoContent, NotFound<ProblemDetails>>> HandleAsync(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(new DeleteCategoryCommand(id), cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new() { Detail = $"Category with id {id} not found." }
            )
            : TypedResults.NoContent();
    }
}
