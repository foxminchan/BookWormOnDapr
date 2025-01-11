namespace BookWor.Constants;

public sealed class ServiceName
{
    public static class Database
    {
        public const string Catalog = "catalogdb";
        public const string Ordering = "orderingdb";
        public const string Rating = "ratingdb";
        public const string Customer = "customerdb";
    }

    public static class Component
    {
        public const string Pubsub = "pubsub";
        public const string Store = "statestore";
    }

    public const string Blob = "blob";
    public const string Bus = "bus";
}
