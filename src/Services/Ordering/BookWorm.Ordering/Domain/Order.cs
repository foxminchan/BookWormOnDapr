using Ardalis.GuardClauses;
using BookWorm.Ordering.Domain.Events;
using BookWorm.SharedKernel.Core.Model;
using BookWorm.SharedKernel.Models;

namespace BookWorm.Ordering.Domain;

public sealed class Order : AuditableEntity, IAggregateRoot, ISoftDelete
{
    public Order()
    {
        _items = [];
    }

    public int No { get; private set; }
    public string? Notes { get; private set; }
    public Guid? ConsumerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public bool IsDeleted { get; set; }

    private readonly List<Item> _items;
    public IReadOnlyCollection<Item> Items => _items.AsReadOnly();

    public Order(string? notes, Guid? consumerId, List<Item> items)
        : this()
    {
        No = new Random().Next(1000, 9999);
        Notes = notes;
        Status = OrderStatus.New;
        ConsumerId = consumerId;
        _items = Guard.Against.Null(items);
        RegisterDomainEvent(new OrderPlacedEvent(Id, Items.Sum(x => x.Price)));
    }

    /// <summary>
    /// Mark the order as paid
    /// </summary>
    public void MarkAsPaid()
    {
        Status = OrderStatus.Paid;
        RegisterDomainEvent(new OrderPaidEvent(Id, Items.Sum(x => x.Price)));
    }

    /// <summary>
    /// Mark the order as cancelled
    /// </summary>
    public void MarkAsCancelled()
    {
        Status = OrderStatus.Cancelled;
        RegisterDomainEvent(new OrderCancelledEvent(Id, Items.Sum(x => x.Price)));
    }

    /// <summary>
    /// Mark the order as completed
    /// </summary>
    public void MarkAsCompleted()
    {
        Status = OrderStatus.Completed;
        RegisterDomainEvent(new OrderCompletedEvent(Id, Items.Sum(x => x.Price)));
    }

    /// <summary>
    /// Delete the order
    /// </summary>
    public void Delete()
    {
        IsDeleted = true;
    }
}
