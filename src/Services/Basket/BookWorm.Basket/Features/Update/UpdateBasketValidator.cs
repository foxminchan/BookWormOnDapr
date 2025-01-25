namespace BookWorm.Basket.Features.Update;

internal sealed class UpdateBasketValidator : AbstractValidator<UpdateBasketCommand>
{
    public UpdateBasketValidator()
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
