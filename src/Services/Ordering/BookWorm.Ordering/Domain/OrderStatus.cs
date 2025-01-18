using System.Text.Json.Serialization;

namespace BookWorm.Ordering.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    New = 1,
    Paid = 2,
    Cancelled = 3,
    Completed = 4,
}
