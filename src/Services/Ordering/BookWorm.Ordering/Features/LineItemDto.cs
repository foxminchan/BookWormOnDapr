namespace BookWorm.Ordering.Features;

public sealed record LineItemDto(Guid ProductId, int Quantity, decimal Price);
