namespace BookWorm.Ordering.Contracts;

public sealed record ApprovalRequest(Guid OrderId, decimal Total, Guid? CustomerId);
