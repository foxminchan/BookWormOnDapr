using BookWorm.SharedKernel.EventBus.Events;

namespace BookWorm.SharedKernel.EventBus.Abstractions;

public interface IEventBus
{
    Task PublishAsync(
        IntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default
    );
}
