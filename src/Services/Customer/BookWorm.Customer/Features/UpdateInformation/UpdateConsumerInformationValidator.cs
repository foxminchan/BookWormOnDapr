using BookWorm.Constants;
using FluentValidation;

namespace BookWorm.Customer.Features.Update;

internal sealed class UpdateConsumerInformationValidator
    : AbstractValidator<UpdateConsumerInformationCommand>
{
    public UpdateConsumerInformationValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(DataSchemaLength.Medium);

        RuleFor(x => x.LastName).NotEmpty().MaximumLength(DataSchemaLength.Medium);

        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(
                @"^(1[ \-\+]{0,3}|\+1[ -\+]{0,3}|\+1|\+)?((\(\+?1-[2-9][0-9]{1,2}\))|(\(\+?[2-8][0-9][0-9]\))|(\(\+?[1-9][0-9]\))|(\(\+?[17]\))|(\([2-9][2-9]\))|([ \-\.]{0,3}[0-9]{2,4}))?([ \-\.][0-9])?([ \-\.]{0,3}[0-9]{2,4}){2,3}$"
            )
            .WithMessage("Invalid phone number format.")
            .MaximumLength(DataSchemaLength.Large);

        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow.Date))
            .When(x => x.DateOfBirth.HasValue);
    }
}
