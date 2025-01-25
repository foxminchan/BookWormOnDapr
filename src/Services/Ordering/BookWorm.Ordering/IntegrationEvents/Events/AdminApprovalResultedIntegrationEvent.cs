using BookWorm.Ordering.Contracts;

namespace BookWorm.Ordering.IntegrationEvents.Events;

public sealed record AdminApprovalResultedIntegrationEvent(Guid OrderId, Approval ApprovalResult)
    : IntegrationEvent;
