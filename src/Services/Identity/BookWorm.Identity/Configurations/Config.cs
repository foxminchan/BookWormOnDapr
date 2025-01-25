namespace BookWorm.Identity.Configurations;

public static class Config
{
    // Identity resources are data like user ID, name, or email address of a user
    // see: https://docs.duendesoftware.com/identityserver/v7/reference/models/api_resource/
    public static IEnumerable<IdentityResource> GetResources() =>
        [new IdentityResources.OpenId(), new IdentityResources.Profile()];

    // ApiResources define the apis in your system
    public static IEnumerable<ApiResource> GetApis() =>
        [
            new("ordering", "Ordering Service"),
            new("basket", "Basket Service"),
            new("catalog", "Catalog Service"),
            new("rating", "Rating Service"),
            new("inventory", "Inventory Service"),
            new("customer", "Customer Service"),
        ];

    // ApiScope is used to protect the API
    public static IEnumerable<ApiScope> GetApiScopes() =>
        [
            new("ordering", "Ordering Service"),
            new("basket", "Basket Service"),
            new("catalog", "Catalog Service"),
            new("rating", "Rating Service"),
            new("inventory", "Inventory Service"),
            new("customer", "Customer Service"),
        ];

    public static IEnumerable<Client> GetClients(ServiceOptions service) =>
        [
            new()
            {
                ClientId = "catalog-api",
                ClientName = "Catalog API",
                ClientSecrets = { new("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RequireConsent = false,
                RequirePkce = true,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = { $"{service.Catalog}/scalar/v1" },
                AllowedCorsOrigins = { service.Catalog },
                AllowedScopes = { "catalog" },
            },
            new()
            {
                ClientId = "ordering-api",
                ClientName = "Ordering API",
                ClientSecrets = { new("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RequireConsent = false,
                RequirePkce = true,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = { $"{service.Ordering}/scalar/v1" },
                AllowedCorsOrigins = { service.Ordering },
                AllowedScopes = { "ordering" },
            },
            new()
            {
                ClientId = "basket-api",
                ClientName = "Basket API",
                ClientSecrets = { new("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RequireConsent = false,
                RequirePkce = true,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = { $"{service.Basket}/scalar/v1" },
                AllowedCorsOrigins = { service.Basket },
                AllowedScopes = { "basket" },
            },
            new()
            {
                ClientId = "rating-api",
                ClientName = "Rating API",
                ClientSecrets = { new("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RequireConsent = false,
                RequirePkce = true,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = { $"{service.Rating}/scalar/v1" },
                AllowedCorsOrigins = { service.Rating },
                AllowedScopes = { "rating" },
            },
            new()
            {
                ClientId = "inventory-api",
                ClientName = "Inventory API",
                ClientSecrets = { new("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RequireConsent = false,
                RequirePkce = true,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = { $"{service.Inventory}/scalar/v1" },
                AllowedCorsOrigins = { service.Inventory },
                AllowedScopes = { "inventory" },
            },
            new()
            {
                ClientId = "customer-api",
                ClientName = "Customer API",
                ClientSecrets = { new("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RequireConsent = false,
                RequirePkce = true,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = { $"{service.Customer}/scalar/v1" },
                AllowedCorsOrigins = { service.Customer },
                AllowedScopes = { "customer" },
            },
            new()
            {
                ClientId = "bff",
                ClientName = "Backend For Frontend",
                ClientSecrets = { new("secret".Sha256()) },
                AllowedGrantTypes = [GrantType.AuthorizationCode],
                RedirectUris = { $"{service.Bff}/signin-oidc" },
                BackChannelLogoutUri = $"{service.Bff}/bff/backchannel",
                PostLogoutRedirectUris = { $"{service.Bff}/signout-callback-oidc" },
                AllowedCorsOrigins = { $"{service.Bff}" },
                AllowOfflineAccess = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    "catalog",
                    "ordering",
                    "basket",
                    "rating",
                    "inventory",
                    "customer",
                },
                AccessTokenLifetime = 60 * 60 * 2,
                IdentityTokenLifetime = 60 * 60 * 2,
            },
        ];
}
