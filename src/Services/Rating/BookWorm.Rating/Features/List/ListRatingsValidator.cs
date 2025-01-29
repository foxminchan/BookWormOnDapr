namespace BookWorm.Rating.Features.List;

internal sealed class ListRatingsValidator : AbstractValidator<ListRatingsQuery>
{
    public ListRatingsValidator()
    {
        RuleFor(x => x.BookId).NotEmpty();

        RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
    }
}
