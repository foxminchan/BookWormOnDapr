using System.ComponentModel;
using Ardalis.Result;
using BookWorm.Catalog.Domain.BookAggregate;
using BookWorm.Catalog.Domain.BookAggregate.Specifications;
using BookWorm.Catalog.Infrastructure.Blob;
using BookWorm.Constants;
using BookWorm.SharedKernel.Query;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Catalog.Features.Books.List;

internal sealed record ListBooksQuery(
    [property: Description("Number of items to return in a single page of results")]
    [property: DefaultValue(Pagination.DefaultPageIndex)]
        int PageIndex = Pagination.DefaultPageIndex,
    [property: Description("Number of items to return in a single page of results")]
    [property: DefaultValue(Pagination.DefaultPageSize)]
        int PageSize = Pagination.DefaultPageSize,
    [property: Description("Property to order results by")]
    [property: DefaultValue(nameof(Book.Name))]
        string? OrderBy = nameof(Book.Name),
    [property: Description("Whether to order results in descending order")]
    [property: DefaultValue(false)]
        bool IsDescending = false,
    [property: Description("Search term to filter results by")]
    [property: DefaultValue(null)]
        string? Search = null,
    [property: Description("Minimum price to filter results by")]
    [property: DefaultValue(null)]
        decimal? MinPrice = null,
    [property: Description("Maximum price to filter results by")]
    [property: DefaultValue(null)]
        decimal? MaxPrice = null,
    [property: Description("Category IDs to filter results by")]
    [property: DefaultValue(null)]
        Guid[]? CategoryId = null,
    [property: Description("Author IDs to filter results by")]
    [property: DefaultValue(null)]
        Guid[]? AuthorIds = null
) : IQuery<PagedResult<IReadOnlyList<BookDto>>>;

internal sealed class ListBooksHandler(IReadRepository<Book> repository, IBlobService blobService)
    : IQueryHandler<ListBooksQuery, PagedResult<IReadOnlyList<BookDto>>>
{
    public async Task<PagedResult<IReadOnlyList<BookDto>>> Handle(
        ListBooksQuery request,
        CancellationToken cancellationToken
    )
    {
        var books = repository.ListAsync(
            new BookFilterSpec(
                request.PageIndex,
                request.PageSize,
                request.OrderBy,
                request.IsDescending,
                request.Search,
                request.MinPrice,
                request.MaxPrice,
                request.CategoryId,
                request.AuthorIds
            ),
            cancellationToken
        );

        var totalRecords = repository.CountAsync(
            new BookFilterSpec(
                request.Search,
                request.MinPrice,
                request.MaxPrice,
                request.CategoryId,
                request.AuthorIds
            ),
            cancellationToken
        );

        await Task.WhenAll(books, totalRecords);

        var imageUrls = books
            .Result.Where(book => book.ImageUrl is not null)
            .ToDictionary(book => book.Id, book => blobService.GetFileUrl(book.ImageUrl!));

        var bookDtos = books.Result.ToBookDtos(imageUrls);

        var totalPages = (int)Math.Ceiling(totalRecords.Result / (double)request.PageSize);

        PagedInfo pagedInfo = new(
            request.PageIndex,
            request.PageSize,
            totalRecords.Result,
            totalPages
        );

        return new(pagedInfo, bookDtos);
    }
}
