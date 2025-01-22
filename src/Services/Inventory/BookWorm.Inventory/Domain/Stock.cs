using Ardalis.GuardClauses;
using BookWorm.SharedKernel.Core.Model;

namespace BookWorm.Inventory.Domain;

public sealed class Stock : Entity<long>
{
    public Stock()
    {
        // EF Core
    }

    public long WarehouseId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    public Stock(Guid productId, int quantity)
        : this()
    {
        ProductId = Guard.Against.Default(productId);
        Quantity = Guard.Against.NegativeOrZero(quantity);
    }

    /// <summary>
    /// Increase the quantity of the stock
    /// </summary>
    /// <param name="quantity">Number of items to increase</param>
    public void IncreaseQuantity(int quantity)
    {
        Quantity += Guard.Against.NegativeOrZero(quantity);
    }

    /// <summary>
    /// Decrease the quantity of the stock
    /// </summary>
    /// <param name="quantity">Number of items to decrease</param>
    public void DecreaseQuantity(int quantity)
    {
        Quantity -= Guard.Against.NegativeOrZero(quantity);
    }
}
