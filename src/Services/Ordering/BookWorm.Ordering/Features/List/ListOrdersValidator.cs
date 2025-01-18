using FluentValidation;

namespace BookWorm.Ordering.Features.List;

internal sealed class ListOrdersValidator : AbstractValidator<ListOrdersQuery>
{
    public ListOrdersValidator()
    {
        RuleFor(query => query.PageIndex).GreaterThanOrEqualTo(0);

        RuleFor(query => query.PageSize).GreaterThanOrEqualTo(1);
    }
}
