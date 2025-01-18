using Aspire.Hosting.Dapr;
using BookWorm.Constants;
using Microsoft.Extensions.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgresUser = builder.AddParameter("SqlUser", true);
var postgresPassword = builder.AddParameter("SqlPassword", true);
var rabbitUser = builder.AddParameter("RabbitUser");
var rabbitPass = builder.AddParameter("RabbitPassword", true);

var postgres = builder
    .AddPostgres("postgres", postgresUser, postgresPassword, 5432)
    .WithPgAdmin()
    .WithDataBindMount("../../../mnt/postgres")
    .WithLifetime(ContainerLifetime.Persistent);

var catalogDb = builder.AddPostgres(ServiceName.Database.Catalog);
var orderingDb = builder.AddPostgres(ServiceName.Database.Ordering);
var ratingDb = builder.AddPostgres(ServiceName.Database.Rating);
var customerDb = builder.AddPostgres(ServiceName.Database.Customer);
var inventoryDb = builder.AddPostgres(ServiceName.Database.Inventory);

var storage = builder.AddAzureStorage("storage");

if (builder.Environment.IsDevelopment())
{
    storage.RunAsEmulator(config => config.WithDataBindMount("../../../mnt/azurite"));
}

var blobs = storage.AddBlobs(ServiceName.Blob);

var rabbitMq = builder
    .AddRabbitMQ(ServiceName.Bus, rabbitUser, rabbitPass)
    .WithManagementPlugin()
    .WithEndpoint("tcp", e => e.Port = 5672)
    .WithEndpoint("management", e => e.Port = 15672);

var stateStore = builder.AddDaprStateStore(
    ServiceName.Component.Store,
    new DaprComponentOptions { LocalPath = "../../../dapr/components/statestore.yaml" }
);

var pubSub = builder
    .AddDaprPubSub(
        ServiceName.Component.Pubsub,
        new DaprComponentOptions { LocalPath = "../../../dapr/components/pubsub.yaml" }
    )
    .WaitFor(rabbitMq);

var keycloak = builder
    .AddKeycloak(ServiceName.Keycloak, 5000)
    .WithDataBindMount("../../../mnt/keycloak")
    .WithExternalHttpEndpoints();

var daprOptions = new DaprSidecarOptions
{
    LogLevel = "debug",
    Config = Path.Combine(
        Directory.GetCurrentDirectory(),
        "../../../dapr/configuration/config.yaml"
    ),
};

builder
    .AddExecutable("dapr-dashboard", "dapr", ".", "dashboard")
    .WithHttpEndpoint(8080, 8080, "dapr-dashboard", isProxied: false)
    .ExcludeFromManifest();

var catalogApi = builder
    .AddProject<BookWorm_Catalog>("bookworm-catalog")
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 3500 }))
    .WithReference(catalogDb)
    .WithReference(blobs)
    .WithReference(pubSub)
    .WithReference(keycloak)
    .WaitFor(blobs)
    .WaitFor(catalogDb)
    .WaitFor(keycloak);

var basketApi = builder
    .AddProject<BookWorm_Basket>("bookworm-basket")
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 3600 }))
    .WithReference(pubSub)
    .WithReference(keycloak)
    .WithReference(stateStore)
    .WaitFor(keycloak);

var orderingApi = builder
    .AddProject<BookWorm_Ordering>("bookworm-ordering")
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 3700 }))
    .WithReference(orderingDb)
    .WithReference(pubSub)
    .WithReference(keycloak)
    .WaitFor(orderingDb)
    .WaitFor(keycloak);

var ratingApi = builder
    .AddProject<BookWorm_Rating>("bookworm-rating")
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 3800 }))
    .WithReference(ratingDb)
    .WithReference(pubSub)
    .WithReference(keycloak)
    .WaitFor(ratingDb)
    .WaitFor(keycloak);

var customerApi = builder
    .AddProject<BookWorm_Customer>("bookworm-customer")
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 3900 }))
    .WithReference(customerDb)
    .WithReference(pubSub)
    .WithReference(keycloak)
    .WaitFor(customerDb)
    .WaitFor(keycloak);

var inventoryApi = builder
    .AddProject<BookWorm_Inventory>("bookworm-inventory")
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 4000 }))
    .WithReference(inventoryDb)
    .WithReference(pubSub)
    .WithReference(keycloak)
    .WaitFor(inventoryDb)
    .WaitFor(keycloak);

builder
    .AddProject<BookWorm_Notification>("bookworm-notification")
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 4100 }));

builder
    .AddProject<BookWorm_ApiGateway>("bookworm-apigateway")
    .WithReference(catalogApi)
    .WithReference(basketApi)
    .WithReference(orderingApi)
    .WithReference(ratingApi)
    .WithReference(customerApi)
    .WithReference(inventoryApi)
    .WaitFor(catalogApi)
    .WaitFor(basketApi)
    .WaitFor(orderingApi)
    .WaitFor(ratingApi)
    .WaitFor(customerApi)
    .WaitFor(inventoryApi);

builder.Build().Run();
