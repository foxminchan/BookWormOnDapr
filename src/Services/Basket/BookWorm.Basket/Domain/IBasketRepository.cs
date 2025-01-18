namespace BookWorm.Basket.Domain;

public interface IBasketRepository
{
    Task<Guid> CreateAsync(Card card, CancellationToken cancellationToken = default);
    Task<Card> GetAsync(Guid? customerId, CancellationToken cancellationToken = default);
    Task UpdateAsync(Card card, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid? customerId, CancellationToken cancellationToken = default);
}
