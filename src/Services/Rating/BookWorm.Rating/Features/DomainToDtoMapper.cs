using BookWorm.Rating.Domain;

namespace BookWorm.Rating.Features;

public static class DomainToDtoMapper
{
    public static ReviewDto ToReviewDto(this Review review, string? reviewer)
    {
        return new(
            review.Id,
            review.BookId,
            reviewer ?? "Anonymous",
            review.Comment,
            review.Rating,
            review.CreatedAt
        );
    }

    public static IReadOnlyList<ReviewDto> ToReviewDtos(
        this IReadOnlyCollection<Review> reviews,
        Dictionary<Guid, string> reviewers
    )
    {
        return reviews.Select(review => review.ToReviewDto(reviewers[review.CustomerId])).ToList();
    }
}
