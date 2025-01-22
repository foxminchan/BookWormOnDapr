using FluentValidation;

namespace BookWorm.Inventory.Features.Stocks.Remove;

internal sealed class RemoveStockValidator : AbstractValidator<RemoveStockCommand>
{
    public RemoveStockValidator()
    {
        RuleFor(x => x.WarehouseId).NotEmpty();

        RuleFor(x => x.Quantity).GreaterThan(0);

        RuleFor(x => x.ProductId).NotEmpty();
    }
}
