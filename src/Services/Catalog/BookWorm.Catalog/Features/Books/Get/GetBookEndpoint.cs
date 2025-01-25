using BookWorm.Catalog.Domain.BookAggregate;

namespace BookWorm.Catalog.Features.Books.Get;

internal sealed class GetBookEndpoint
    : IEndpoint<Results<Ok<BookDto>, NotFound<ProblemDetails>>, Guid, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/books/{id:guid}",
                async ([Description("The book id")] Guid id, ISender sender) =>
                    await HandleAsync(id, sender)
            )
            .Produces<BookDto>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .WithTags(nameof(Book))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Results<Ok<BookDto>, NotFound<ProblemDetails>>> HandleAsync(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(new GetBookQuery(id), cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new() { Detail = $"Book with id {id} not found." }
            )
            : TypedResults.Ok(result.Value);
    }
}
