using FluentValidation;

namespace BookWorm.Catalog.Features.Books.List;

internal sealed class ListBooksValidator : AbstractValidator<ListBooksQuery>
{
    public ListBooksValidator()
    {
        RuleFor(query => query.PageIndex).GreaterThanOrEqualTo(0);

        RuleFor(query => query.PageSize).GreaterThanOrEqualTo(1);
    }
}
