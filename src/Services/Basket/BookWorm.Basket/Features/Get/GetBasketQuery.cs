using Ardalis.Result;
using BookWorm.Basket.Domain;
using BookWorm.SharedKernel.Query;

namespace BookWorm.Basket.Features.Get;

internal sealed record GetBasketQuery(Guid CustomerId) : IQuery<Result<Card>>;

internal sealed class GetBasketHandler(IBasketRepository repository)
    : IQueryHandler<GetBasketQuery, Result<Card>>
{
    public async Task<Result<Card>> Handle(
        GetBasketQuery request,
        CancellationToken cancellationToken
    )
    {
        var card = await repository.GetAsync(request.CustomerId, cancellationToken);

        if (card is null)
        {
            return Result.NotFound();
        }

        return card;
    }
}
