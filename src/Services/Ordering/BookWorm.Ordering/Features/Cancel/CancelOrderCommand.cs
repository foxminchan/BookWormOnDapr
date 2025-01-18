using Ardalis.Result;
using BookWorm.Ordering.Domain;
using BookWorm.Ordering.Domain.Specifications;
using BookWorm.SharedKernel.Command;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Ordering.Features.Cancel;

internal sealed record CancelOrderCommand(Guid OrderId) : ICommand;

internal sealed class CancelOrderHandler(IRepository<Order> repository)
    : ICommandHandler<CancelOrderCommand>
{
    public async Task<Result> Handle(
        CancelOrderCommand request,
        CancellationToken cancellationToken
    )
    {
        var order = await repository.FirstOrDefaultAsync(
            new OrderFilterSpec(request.OrderId),
            cancellationToken
        );

        if (order is null)
        {
            return Result.NotFound();
        }

        order.MarkAsCancelled();

        await repository.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
