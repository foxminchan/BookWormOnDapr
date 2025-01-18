using Ardalis.GuardClauses;
using BookWorm.SharedKernel.Core.Model;

namespace BookWorm.Ordering.Domain;

public sealed class Item : Entity
{
    public Item()
    {
        // EF Core
    }

    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = default!;

    public Item(Guid id, int quantity, decimal price)
        : this()
    {
        Id = Guard.Against.Default(id, nameof(id));
        Quantity = Guard.Against.NegativeOrZero(quantity);
        Price = Guard.Against.Negative(price);
    }
}
