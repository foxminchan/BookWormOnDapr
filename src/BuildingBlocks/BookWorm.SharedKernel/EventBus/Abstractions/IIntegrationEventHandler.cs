using BookWorm.SharedKernel.EventBus.Events;

namespace BookWorm.SharedKernel.EventBus.Abstractions;

public interface IIntegrationEventHandler;

public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event);
}
