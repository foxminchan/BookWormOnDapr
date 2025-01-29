namespace BookWorm.Rating.Domain.Specifications;

public static class ReviewSpecExpression
{
    public static ISpecificationBuilder<Review> ApplyOrdering(
        this ISpecificationBuilder<Review> builder,
        string? orderBy,
        bool isDescending
    )
    {
        return orderBy switch
        {
            nameof(Review.Rating) => isDescending
                ? builder.OrderByDescending(x => x.Rating)
                : builder.OrderBy(x => x.Rating),
            nameof(Review.LastModifiedAt) => isDescending
                ? builder.OrderByDescending(x => x.CreatedAt)
                : builder.OrderBy(x => x.CreatedAt),
            _ => isDescending
                ? builder.OrderByDescending(x => x.Rating)
                : builder.OrderBy(x => x.Rating),
        };
    }

    public static ISpecificationBuilder<Review> ApplyPaging(
        this ISpecificationBuilder<Review> builder,
        int pageIndex,
        int pageSize
    )
    {
        return builder.Skip((pageIndex - 1) * pageSize).Take(pageSize);
    }
}
