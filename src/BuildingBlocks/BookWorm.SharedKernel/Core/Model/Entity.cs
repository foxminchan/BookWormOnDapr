using BookWorm.SharedKernel.Core.Event;

namespace BookWorm.SharedKernel.Core.Model;

public abstract class Entity : HasDomainEventsBase
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
}

public abstract class Entity<TId> : Entity
    where TId : struct, IEquatable<TId>
{
    public new TId Id { get; set; }
}
