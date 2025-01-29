namespace BookWorm.Rating.Features.Create;

internal sealed class CreateRatingValidation : AbstractValidator<CreateRatingCommand>
{
    public CreateRatingValidation()
    {
        RuleFor(x => x.Rating).InclusiveBetween(1, 5);

        RuleFor(x => x.Comment).NotEmpty().MaximumLength(DataSchemaLength.Max);

        RuleFor(x => x.BookId).NotEmpty();
    }
}
