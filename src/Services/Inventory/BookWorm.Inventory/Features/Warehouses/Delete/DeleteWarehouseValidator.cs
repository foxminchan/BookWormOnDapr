using BookWorm.Inventory.Domain;
using BookWorm.SharedKernel.Repositories;
using FluentValidation;

namespace BookWorm.Inventory.Features.Warehouses.Delete;

internal sealed class DeleteWarehouseValidator : AbstractValidator<DeleteWarehouseCommand>
{
    public DeleteWarehouseValidator(IReadRepository<Warehouse> repository)
    {
        RuleFor(x => x.Id)
            .MustAsync(
                async (id, cancellationToken) =>
                {
                    var warehouse = await repository.GetByIdAsync(id, cancellationToken);

                    if (warehouse?.Stocks.Count > 0)
                    {
                        return false;
                    }

                    return warehouse is not null;
                }
            )
            .WithMessage("Warehouse not found or has stocks.");
    }
}
