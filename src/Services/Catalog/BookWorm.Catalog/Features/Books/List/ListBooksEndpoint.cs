using BookWorm.Catalog.Domain.BookAggregate;

namespace BookWorm.Catalog.Features.Books.List;

internal sealed class ListBooksEndpoint
    : IEndpoint<Ok<PagedResult<IReadOnlyList<BookDto>>>, ListBooksQuery, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/books",
                async ([AsParameters] ListBooksQuery query, ISender sender) =>
                    await HandleAsync(query, sender)
            )
            .ProducesValidationProblem()
            .Produces<PagedResult<IReadOnlyList<BookDto>>>()
            .WithOpenApi()
            .WithTags(nameof(Book))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Ok<PagedResult<IReadOnlyList<BookDto>>>> HandleAsync(
        ListBooksQuery query,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(result);
    }
}
