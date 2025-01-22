using FluentValidation;

namespace BookWorm.Inventory.Features.Stocks.Add;

internal sealed class AddStocksValidator : AbstractValidator<AddStocksCommand>
{
    public AddStocksValidator()
    {
        RuleFor(x => x.WarehouseId).NotEmpty();

        RuleFor(x => x.Items)
            .NotEmpty()
            .ForEach(stock =>
            {
                stock.ChildRules(stock =>
                {
                    stock.RuleFor(x => x.ProductId).NotEmpty();
                    stock.RuleFor(x => x.Quantity).GreaterThan(0);
                });
            });
    }
}
