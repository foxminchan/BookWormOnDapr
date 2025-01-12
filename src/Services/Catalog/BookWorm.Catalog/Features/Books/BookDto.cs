namespace BookWorm.Catalog.Features.Books;

public sealed record BookDto(
    Guid Id,
    string? Name,
    string? Description,
    string? ImageUrl,
    decimal Price,
    Guid[] AuthorIds,
    Guid[] CategoryIds
);
