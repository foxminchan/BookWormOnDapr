using Ardalis.Specification;

namespace BookWorm.Catalog.Domain.BookAggregate.Specifications;

public sealed class BookFilterSpec : Specification<Book>
{
    public BookFilterSpec(
        string? search,
        decimal? minPrice,
        decimal? maxPrice,
        Guid[]? categoryId,
        Guid[]? authorIds
    )
    {
        Query.Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            Query.Where(x => x.Name!.Contains(search) || x.Description!.Contains(search));
        }

        if (minPrice.HasValue)
        {
            Query.Where(x => x.Price >= minPrice);
        }

        if (maxPrice.HasValue)
        {
            Query.Where(x => x.Price <= maxPrice);
        }

        if (categoryId is not null && categoryId.Length > 0)
        {
            Query.Where(x => x.BookCategories.Any(y => categoryId.Contains(y.CategoryId)));
        }

        if (authorIds is not null && authorIds.Length > 0)
        {
            Query.Where(x => x.BookAuthors.Any(y => authorIds.Contains(y.AuthorId)));
        }
    }

    public BookFilterSpec(
        int pageIndex,
        int pageSize,
        string? orderBy,
        bool isDescending,
        string? search,
        decimal? minPrice,
        decimal? maxPrice,
        Guid[]? categoryId,
        Guid[]? authorIds
    )
        : this(search, minPrice, maxPrice, categoryId, authorIds)
    {
        Query.ApplyOrdering(orderBy, isDescending).ApplyPaging(pageIndex, pageSize);
    }

    public BookFilterSpec(Guid id)
    {
        Query.Where(x => x.Id == id && !x.IsDeleted);
    }
}
