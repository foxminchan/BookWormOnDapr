using BookWorm.Catalog.Domain;

namespace BookWorm.Catalog.Features.Authors;

public static class DomainToDtoMapper
{
    public static AuthorDto ToAuthorDto(this Author author)
    {
        return new AuthorDto(author.Id, author.Name);
    }

    public static IReadOnlyList<AuthorDto> ToAuthorDtos(this List<Author>? authors)
    {
        return authors?.Select(ToAuthorDto).ToList() ?? [];
    }
}
