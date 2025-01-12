using Ardalis.Specification;

namespace BookWorm.Catalog.Domain.BookAggregate.Specifications;

public static class BookSpecExpression
{
    public static ISpecificationBuilder<Book> ApplyOrdering(
        this ISpecificationBuilder<Book> builder,
        string? orderBy,
        bool isDescending
    )
    {
        return orderBy switch
        {
            nameof(Book.Name) => isDescending
                ? builder.OrderByDescending(x => x.Name)
                : builder.OrderBy(x => x.Name),
            nameof(Book.Price) => isDescending
                ? builder.OrderByDescending(x => x.Price)
                : builder.OrderBy(x => x.Price),
            nameof(Book.CreatedAt) => isDescending
                ? builder.OrderByDescending(x => x.CreatedAt)
                : builder.OrderBy(x => x.CreatedAt),
            _ => isDescending
                ? builder.OrderByDescending(x => x.Price)
                : builder.OrderBy(x => x.Price),
        };
    }

    public static ISpecificationBuilder<Book> ApplyPaging(
        this ISpecificationBuilder<Book> builder,
        int pageIndex,
        int pageSize
    )
    {
        return builder.Skip((pageIndex - 1) * pageSize).Take(pageSize);
    }
}
