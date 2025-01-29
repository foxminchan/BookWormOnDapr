namespace BookWorm.Ordering.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus : byte
{
    New = 1,
    Paid = 2,
    Cancelled = 3,
    Completed = 4,
}
