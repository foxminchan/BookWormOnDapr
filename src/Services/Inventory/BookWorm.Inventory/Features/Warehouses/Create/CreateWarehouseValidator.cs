namespace BookWorm.Inventory.Features.Warehouses.Create;

internal sealed class CreateWarehouseValidator : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseValidator()
    {
        RuleFor(x => x.Location).NotEmpty().MaximumLength(DataSchemaLength.ExtraLarge);

        RuleFor(x => x.Status).IsInEnum();

        RuleFor(x => x.Description).MaximumLength(DataSchemaLength.Max);

        RuleFor(x => x.Website).MaximumLength(DataSchemaLength.SuperLarge);

        RuleFor(x => x.Stocks)
            .NotEmpty()
            .ForEach(stock =>
            {
                stock.ChildRules(rules =>
                {
                    rules.RuleFor(x => x.ProductId).NotEmpty();
                    rules.RuleFor(x => x.Quantity).GreaterThan(0);
                });
            });
    }
}
