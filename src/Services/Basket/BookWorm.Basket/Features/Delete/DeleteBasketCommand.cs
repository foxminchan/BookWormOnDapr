using Ardalis.Result;
using BookWorm.Basket.Domain;
using BookWorm.SharedKernel.Command;

namespace BookWorm.Basket.Features.Delete;

internal sealed record DeleteBasketCommand(Guid CustomerId) : ICommand;

internal sealed class DeleteBasketHandler(IBasketRepository repository)
    : ICommandHandler<DeleteBasketCommand>
{
    public async Task<Result> Handle(
        DeleteBasketCommand request,
        CancellationToken cancellationToken
    )
    {
        var card = await repository.GetAsync(request.CustomerId, cancellationToken);

        if (card is null)
        {
            return Result.NotFound();
        }

        await repository.DeleteAsync(request.CustomerId, cancellationToken);

        return Result.NoContent();
    }
}
