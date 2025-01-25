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

    /// <summary>
    /// Add item to the card
    /// </summary>
    /// <param name="item">Item to add</param>
    public void AddItem(Item item)
    {
        Items.Add(item);
    }

    /// <summary>
    /// Delete item from the card
    /// </summary>
    /// <param name="itemId">ID of the item to delete</param>
    public void RemoveItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item is not null)
        {
            Items.Remove(item);
        }
    }

    /// <summary>
    /// Increase the quantity of the item
    /// </summary>
    /// <param name="itemId">ID of the item</param>
    /// <param name="quantity">Amount to increase</param>
    public void IncreaseItemQuantity(Guid itemId, int quantity)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        item?.IncreaseQuantity(quantity);
    }

    /// <summary>
    /// Reduce the quantity of the item
    /// </summary>
    /// <param name="itemId">ID of the item</param>
    /// <param name="quantity">Amount to reduce</param>
    public void ReduceItemQuantity(Guid itemId, int quantity)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        item?.ReduceQuantity(quantity);
    }
}
