using BookWorm.SharedKernel.Core.Model;

namespace BookWorm.Basket.Domain;

public sealed class Card : IAggregateRoot
{
    public Card()
    {
    }

    public Guid CustomerId { get; set; }
    public ICollection<Item> Items { get; set; } = [];

    public Card(Guid customerId, List<Item> items) : this()
    {
        CustomerId = customerId;
        Items = items;
    }

    public void AddItem(Item item)
    {
        Items.Add(item);
    }

    public void RemoveItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item is not null)
        {
            Items.Remove(item);
        }
    }

    public void IncreaseItemQuantity(Guid itemId, int quantity)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        item?.IncreaseQuantity(quantity);
    }

    public void ReduceItemQuantity(Guid itemId, int quantity)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        item?.ReduceQuantity(quantity);
    }
}
