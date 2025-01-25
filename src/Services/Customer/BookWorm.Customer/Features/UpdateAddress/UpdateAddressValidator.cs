namespace BookWorm.Customer.Features.UpdateAddress;

internal sealed class UpdateAddressValidator : AbstractValidator<UpdateAddressCommand>
{
    public UpdateAddressValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Street).NotEmpty().MaximumLength(DataSchemaLength.Large);

        RuleFor(x => x.City).NotEmpty().MaximumLength(DataSchemaLength.Medium);

        RuleFor(x => x.State).NotEmpty().MaximumLength(DataSchemaLength.Medium);

        RuleFor(x => x.Country).NotEmpty().MaximumLength(DataSchemaLength.Medium);

        RuleFor(x => x.ZipCode).NotEmpty().MaximumLength(DataSchemaLength.Small);
    }
}
