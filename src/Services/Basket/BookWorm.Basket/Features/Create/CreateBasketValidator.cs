namespace BookWorm.Basket.Features.Create;

internal sealed class CreateBasketValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();

        RuleFor(x => x.Items)
            .NotEmpty()
            .ForEach(x =>
                x.ChildRules(item =>
                {
                    item.RuleFor(y => y.Id).NotEmpty();
                    item.RuleFor(y => y.Quantity).GreaterThan(0);
                })
            );
    }
}
