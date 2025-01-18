using System.Text.Json.Serialization;

namespace BookWorm.Ordering.Contracts;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Approval
{
    Unspecified = 0,
    Approved = 1,
    Rejected = 2,
}
