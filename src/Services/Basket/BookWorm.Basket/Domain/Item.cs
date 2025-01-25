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

    /// <summary>
    /// Increase the quantity of the item
    /// </summary>
    /// <param name="quantity">Quantity to increase</param>
    public void IncreaseQuantity(int quantity)
    {
        Quantity += Guard.Against.OutOfRange(quantity, nameof(quantity), 1, int.MaxValue);
    }

    /// <summary>
    /// Reduce the quantity of the item
    /// </summary>
    /// <param name="quantity">Quantity to reduce</param>
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
