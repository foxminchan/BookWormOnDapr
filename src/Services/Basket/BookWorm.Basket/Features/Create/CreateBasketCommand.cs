using BookWorm.Basket.Domain;

namespace BookWorm.Basket.Features.Create;

internal sealed record CreateBasketCommand(Guid CustomerId, List<Item> Items)
    : ICommand<Result<Guid>>;

internal sealed class CreateBasketHandler(IBasketRepository repository)
    : ICommandHandler<CreateBasketCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateBasketCommand request,
        CancellationToken cancellationToken
    )
    {
        var card = new Card(request.CustomerId, request.Items);

        var customerId = await repository.CreateAsync(card, cancellationToken);

        return customerId;
    }
}
