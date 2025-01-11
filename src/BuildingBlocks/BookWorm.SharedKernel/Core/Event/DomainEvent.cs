using MediatR;

namespace BookWorm.SharedKernel.Core.Event;

public abstract class DomainEvent : INotification
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}
