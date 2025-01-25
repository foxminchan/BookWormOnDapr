using BookWorm.Catalog.Domain.BookAggregate;

namespace BookWorm.Catalog.Features.Books.Update;

internal sealed class UpdateBookEndpoint
    : IEndpoint<Results<NoContent, NotFound<ProblemDetails>>, UpdateBookCommand, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/books",
                async ([FromForm] UpdateBookCommand command, ISender sender) =>
                    await HandleAsync(command, sender)
            )
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .DisableAntiforgery()
            .WithOpenApi()
            .WithTags(nameof(Book))
            .MapToApiVersion(new(1, 0))
            .WithFormOptions(bufferBody: true);
    }

    public async Task<Results<NoContent, NotFound<ProblemDetails>>> HandleAsync(
        UpdateBookCommand command,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(command, cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new() { Detail = $"Book with id {command.Id} not found." }
            )
            : TypedResults.NoContent();
    }
}
