using Ardalis.Result;
using BookWorm.Catalog.Domain.BookAggregate;
using BookWorm.Catalog.Domain.BookAggregate.Specifications;
using BookWorm.Catalog.Infrastructure.Blob;
using BookWorm.SharedKernel.Query;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Catalog.Features.Books.Get;

internal sealed record GetBookQuery(Guid Id) : IQuery<Result<BookDto>>;

internal sealed class GetBookHandler(IReadRepository<Book> repository, IBlobService blobService)
    : IQueryHandler<GetBookQuery, Result<BookDto>>
{
    public async Task<Result<BookDto>> Handle(
        GetBookQuery request,
        CancellationToken cancellationToken
    )
    {
        var book = await repository.FirstOrDefaultAsync(
            new BookFilterSpec(request.Id),
            cancellationToken
        );

        if (book is null)
        {
            return Result.NotFound();
        }

        string? imageUrl = null;

        if (book.ImageUrl is not null)
        {
            imageUrl = blobService.GetFileUrl(book.ImageUrl);
        }

        return book.ToBookDto(imageUrl);
    }
}
