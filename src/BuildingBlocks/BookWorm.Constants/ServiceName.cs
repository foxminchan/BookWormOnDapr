﻿namespace BookWorm.Constants;

public sealed class ServiceName
{
    // Database Definitions
    public static class Database
    {
        public const string Catalog = "catalogdb";
        public const string Ordering = "orderingdb";
        public const string Rating = "ratingdb";
        public const string Customer = "customerdb";
        public const string Inventory = "inventorydb";
    }

    // Dapr Component Definitions
    public static class Component
    {
        public const string Pubsub = "pubsub";
        public const string Store = "statestore";
    }

    // Infrastructure Definitions
    public const string Blob = "blob";
    public const string Bus = "bus";
    public const string Keycloak = "keycloak";

    private const string Prefix = "bookworm";

    // Application Definitions
    public static class App
    {
        public const string Catalog = Prefix + "-catalog";
        public const string Ordering = Prefix + "-ordering";
        public const string Basket = Prefix + "-basket";
        public const string Rating = Prefix + "-rating";
        public const string Customer = Prefix + "-customer";
        public const string Inventory = Prefix + "-inventory";
        public const string Notification = Prefix + "-notification";
        public const string Gateway = Prefix + "-apigateway";
    }
}
