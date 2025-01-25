using BookWorm.Ordering.Domain.Events;
using BookWorm.Ordering.Extensions;
using BookWorm.Ordering.Infrastructure.EventStore.DocumentSession;

namespace BookWorm.Ordering.Domain.EventHandler;

public class OrderEventHandler(IDocumentSession documentSession, ILogger<OrderEventHandler> logger)
    : INotificationHandler<OrderPlacedEvent>,
        INotificationHandler<OrderPaidEvent>,
        INotificationHandler<OrderCancelledEvent>,
        INotificationHandler<OrderCompletedEvent>
{
    public async Task Handle(OrderPlacedEvent @event, CancellationToken cancellationToken)
    {
        OrderingTrace.LogOrderCreated(logger, nameof(OrderPlacedEvent), @event.Id);
        await documentSession.GetAndUpdate<OrderSummary>(
            Guid.CreateVersion7(),
            @event,
            cancellationToken
        );
    }

    public async Task Handle(OrderPaidEvent @event, CancellationToken cancellationToken)
    {
        OrderingTrace.LogOrderCreated(logger, nameof(OrderPaidEvent), @event.Id);
        await documentSession.GetAndUpdate<OrderSummary>(
            Guid.CreateVersion7(),
            @event,
            cancellationToken
        );
    }

    public async Task Handle(OrderCancelledEvent @event, CancellationToken cancellationToken)
    {
        OrderingTrace.LogOrderCreated(logger, nameof(OrderCancelledEvent), @event.Id);
        await documentSession.GetAndUpdate<OrderSummary>(
            Guid.CreateVersion7(),
            @event,
            cancellationToken
        );
    }

    public async Task Handle(OrderCompletedEvent @event, CancellationToken cancellationToken)
    {
        OrderingTrace.LogOrderCreated(logger, nameof(OrderCompletedEvent), @event.Id);
        await documentSession.GetAndUpdate<OrderSummary>(
            Guid.CreateVersion7(),
            @event,
            cancellationToken
        );
    }
}
