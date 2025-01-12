using FluentValidation;

namespace BookWorm.Catalog.Features.Books.List;

internal sealed class ListBooksValidator : AbstractValidator<ListBooksQuery>
{
    public ListBooksValidator()
    {
        RuleFor(query => query.PageIndex)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Page index must be greater than or equal to 0.");

        RuleFor(query => query.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be greater than or equal to 1.");

        RuleFor(query => query.OrderBy)
            .NotEmpty()
            .WithMessage("Order by property must not be empty.");

        RuleFor(query => query.IsDescending)
            .NotNull()
            .WithMessage("Is descending property must not be null.");
    }
}
