using Ardalis.GuardClauses;

namespace BookWorm.Basket.Domain;

public sealed class Item
{
    public Item()
    {
    }

    public Guid Id { get; set; }
    public int Quantity { get; set; }

    public Item(Guid id, int quantity)
    {
        Id = id;
        Quantity = Guard.Against.OutOfRange(quantity, nameof(quantity), 1, int.MaxValue);
    }

    public void IncreaseQuantity(int quantity)
    {
        Quantity += Guard.Against.OutOfRange(quantity, nameof(quantity), 1, int.MaxValue);
    }

    public void ReduceQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            return;
        }

        if (Quantity - quantity < 0)
        {
            Quantity = 0;
        }
        else
        {
            Quantity -= quantity;
        }
    }
}
