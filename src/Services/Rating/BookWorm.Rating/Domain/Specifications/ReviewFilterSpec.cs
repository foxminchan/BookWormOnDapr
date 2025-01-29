namespace BookWorm.Rating.Domain.Specifications;

public sealed class ReviewFilterSpec : Specification<Review>
{
    public ReviewFilterSpec(Guid bookId)
    {
        Query.Where(x => x.BookId == bookId);
    }

    public ReviewFilterSpec(
        Guid bookId,
        int pageIndex,
        int pageSize,
        string? orderBy,
        bool isDescending
    )
        : this(bookId)
    {
        Query.ApplyOrdering(orderBy, isDescending).ApplyPaging(pageIndex, pageSize);
    }
}
