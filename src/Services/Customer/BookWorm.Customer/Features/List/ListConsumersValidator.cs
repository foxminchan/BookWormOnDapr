using FluentValidation;

namespace BookWorm.Customer.Features.List;

internal sealed class ListConsumersValidator : AbstractValidator<ListConsumersQuery>
{
    public ListConsumersValidator()
    {
        RuleFor(query => query.PageIndex).GreaterThanOrEqualTo(0);

        RuleFor(query => query.PageSize).GreaterThanOrEqualTo(1);

        RuleFor(x => x.Email).EmailAddress();

        RuleFor(x => x.PhoneNumber)
            .Matches(
                @"^(1[ \-\+]{0,3}|\+1[ -\+]{0,3}|\+1|\+)?((\(\+?1-[2-9][0-9]{1,2}\))|(\(\+?[2-8][0-9][0-9]\))|(\(\+?[1-9][0-9]\))|(\(\+?[17]\))|(\([2-9][2-9]\))|([ \-\.]{0,3}[0-9]{2,4}))?([ \-\.][0-9])?([ \-\.]{0,3}[0-9]{2,4}){2,3}$"
            );
    }
}
