namespace BookWorm.Basket.Domain;

public interface IBasketRepository
{
    Task<Guid> CreateAsync(Card card, CancellationToken cancellationToken);
    Task<Card> GetAsync(Guid customerId, CancellationToken cancellationToken);
    Task UpdateAsync(Card card, CancellationToken cancellationToken);
    Task DeleteAsync(Guid customerId, CancellationToken cancellationToken);
}
