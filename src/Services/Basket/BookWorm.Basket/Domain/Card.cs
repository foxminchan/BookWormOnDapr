namespace BookWorm.Basket.Domain;

public sealed class Card() : IAggregateRoot
{
    public Guid CustomerId { get; set; }
    public ICollection<Item> Items { get; set; } = [];

    public Card(Guid customerId, List<Item> items)
        : this()
    {
        CustomerId = customerId;
        Items = items;
    }
}
