namespace BookWorm.Rating.Features;

public sealed record ReviewDto(
    Guid Id,
    Guid BookId,
    string Reviewer,
    string? Content,
    int Rating,
    DateTime CreatedAt
);
