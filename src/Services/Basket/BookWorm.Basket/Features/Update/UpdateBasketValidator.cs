using FluentValidation;

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
                    item.RuleFor(x => x.Id).NotEmpty();
                    item.RuleFor(x => x.Quantity).GreaterThan(0);
                })
            );
    }
}
