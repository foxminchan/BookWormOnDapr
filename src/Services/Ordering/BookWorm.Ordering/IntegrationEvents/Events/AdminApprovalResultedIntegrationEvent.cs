using BookWorm.Ordering.Contracts;
using BookWorm.SharedKernel.EventBus.Events;

namespace BookWorm.Ordering.IntegrationEvents.Events;

public sealed record AdminApprovalResultedIntegrationEvent(Guid OrderId, Approval ApprovalResult)
    : IntegrationEvent;
