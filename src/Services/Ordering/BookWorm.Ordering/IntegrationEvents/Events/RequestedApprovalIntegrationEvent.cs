namespace BookWorm.Ordering.IntegrationEvents.Events;

public sealed record RequestedApprovalIntegrationEvent(Guid OrderId, decimal Total)
    : IntegrationEvent;
