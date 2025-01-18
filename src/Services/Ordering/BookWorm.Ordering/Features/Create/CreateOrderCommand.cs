using Ardalis.Result;
using BookWorm.Ordering.Domain;
using BookWorm.Shared.EF;
using BookWorm.SharedKernel.Command;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Ordering.Features.Create;

internal sealed record CreateOrderCommand(Order Order) : ICommand<Result<Guid>>;

[TxScope]
internal sealed class CreateOrderCommandHandler(IRepository<Order> repository)
    : ICommandHandler<CreateOrderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken
    )
    {
        var result = await repository.AddAsync(request.Order, cancellationToken);

        return result.Id;
    }
}
