namespace BookWorm.Basket.Domain;

public sealed class Item()
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }

    public Item(Guid id, int quantity)
        : this()
    {
        Id = id;
        Quantity = Guard.Against.OutOfRange(quantity, nameof(quantity), 1, int.MaxValue);
    }
}
