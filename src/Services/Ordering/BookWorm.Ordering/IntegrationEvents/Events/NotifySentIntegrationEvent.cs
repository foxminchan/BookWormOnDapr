namespace BookWorm.Ordering.IntegrationEvents.Events;

public sealed record NotifySentIntegrationEvent(Guid? CustomerId, string Message)
    : IntegrationEvent;
