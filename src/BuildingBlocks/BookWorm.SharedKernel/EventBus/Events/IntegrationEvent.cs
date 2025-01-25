namespace BookWorm.SharedKernel.EventBus.Events;

public record IntegrationEvent
{
    public Guid Id { get; } = Guid.CreateVersion7();

    public DateTime CreationDate { get; } = DateTime.UtcNow;
}
