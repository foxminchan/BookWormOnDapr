using Ardalis.Result;
using BookWorm.Basket.Domain;
using BookWorm.SharedKernel.Command;

namespace BookWorm.Basket.Features.Update;

internal sealed record UpdateBasketCommand(Guid CustomerId, List<Item> Items) : ICommand;

internal sealed class UpdateBasketHandler(IBasketRepository repository)
    : ICommandHandler<UpdateBasketCommand>
{
    public async Task<Result> Handle(
        UpdateBasketCommand request,
        CancellationToken cancellationToken
    )
    {
        var card = new Card(request.CustomerId, request.Items);

        await repository.UpdateAsync(card, cancellationToken);

        return Result.Success();
    }
}
