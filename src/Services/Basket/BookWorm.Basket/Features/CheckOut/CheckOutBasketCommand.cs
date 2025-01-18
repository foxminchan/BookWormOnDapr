using Ardalis.Result;
using BookWorm.Basket.Contracts;
using BookWorm.Basket.Domain;
using BookWorm.Basket.IntegrationEvents.Events;
using BookWorm.SharedKernel.Command;
using BookWorm.SharedKernel.EventBus.Abstractions;

namespace BookWorm.Basket.Features.CheckOut;

internal sealed record CheckOutBasketCommand(Guid CustomerId) : ICommand;

internal sealed class CheckOutBasketHandler(IBasketRepository repository, IEventBus eventBus)
    : ICommandHandler<CheckOutBasketCommand>
{
    public async Task<Result> Handle(
        CheckOutBasketCommand request,
        CancellationToken cancellationToken
    )
    {
        var basket = await repository.GetAsync(request.CustomerId, cancellationToken);

        if (basket is null)
        {
            return Result.NotFound();
        }

        var @event = new UserCheckedOutIntegrationEvent(
            basket.CustomerId,
            basket.Items.Select(i => new CardItem(i.Id, i.Quantity)).ToList()
        );

        await eventBus.PublishAsync(@event, cancellationToken);

        return Result.Success();
    }
}
