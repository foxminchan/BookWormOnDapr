using BookWorm.Catalog.Domain.BookAggregate;

namespace BookWorm.Catalog.Features.Books;

public static class DomainToDtoMapper
{
    public static BookDto ToBookDto(this Book book, string? imageUrl)
    {
        return new(
            book.Id,
            book.Name,
            book.Description,
            imageUrl,
            book.Price,
            book.BookAuthors.Select(bookAuthor => bookAuthor.AuthorId).ToArray(),
            book.BookCategories.Select(bookCategory => bookCategory.CategoryId).ToArray()
        );
    }

    public static IReadOnlyList<BookDto> ToBookDtos(
        this List<Book>? books,
        Dictionary<Guid, string> imageUrls
    )
    {
        return books?.Select(book => book.ToBookDto(imageUrls.GetValueOrDefault(book.Id))).ToList()
            ?? [];
    }
}
