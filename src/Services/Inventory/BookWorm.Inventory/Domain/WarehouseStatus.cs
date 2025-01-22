using System.Text.Json.Serialization;

namespace BookWorm.Inventory.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WarehouseStatus : byte
{
    Available = 0,
    AlmostFull = 1,
    Full = 2,
}
