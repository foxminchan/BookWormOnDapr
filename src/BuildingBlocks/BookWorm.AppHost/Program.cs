using Aspire.Hosting.Dapr;
using BookWorm.Constants;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgresUser = builder.AddParameter("SqlUser", true);
var postgresPassword = builder.AddParameter("SqlPassword", true);
var rabbitUser = builder.AddParameter("RabbitUser");
var rabbitPass = builder.AddParameter("RabbitPassword", true);

const string baseDir = "../../..";

var postgres = builder
    .AddPostgres("postgres", postgresUser, postgresPassword, 5432)
    .WithPgAdmin()
    .WithDataBindMount($"{baseDir}/mnt/postgres")
    .WithLifetime(ContainerLifetime.Persistent);

var catalogDb = builder.AddPostgres(ServiceName.Database.Catalog);
var orderingDb = builder.AddPostgres(ServiceName.Database.Ordering);
var ratingDb = builder.AddPostgres(ServiceName.Database.Rating);
var customerDb = builder.AddPostgres(ServiceName.Database.Customer);
var inventoryDb = builder.AddPostgres(ServiceName.Database.Inventory);
var paymentDb = builder.AddPostgres(ServiceName.Database.Payment);

var storage = builder.AddAzureStorage("storage");

if (builder.Environment.IsDevelopment())
{
    storage.RunAsEmulator(config => config.WithDataBindMount($"{baseDir}/mnt/azurite"));
}

var blobs = storage.AddBlobs(ServiceName.Blob);

var rabbitMq = builder
    .AddRabbitMQ(ServiceName.Bus, rabbitUser, rabbitPass)
    .WithManagementPlugin()
    .WithEndpoint("tcp", e => e.Port = 5672)
    .WithEndpoint("management", e => e.Port = 15672);

var stateStore = builder.AddDaprStateStore(
    ServiceName.Component.Store,
    new DaprComponentOptions { LocalPath = $"{baseDir}/dapr/components/statestore.yaml" }
);

var pubSub = builder
    .AddDaprPubSub(
        ServiceName.Component.Pubsub,
        new DaprComponentOptions { LocalPath = $"{baseDir}/dapr/components/pubsub.yaml" }
    )
    .WaitFor(rabbitMq);

var keycloak = builder
    .AddKeycloak(ServiceName.Keycloak, 5000)
    .WithDataBindMount($"{baseDir}/mnt/keycloak")
    .WithExternalHttpEndpoints();

var daprOptions = new DaprSidecarOptions
{
    LogLevel = nameof(LogLevel.Debug),
    Config = Path.Combine(
        Directory.GetCurrentDirectory(),
        $"{baseDir}/dapr/configuration/config.yaml"
    ),
};

builder
    .AddExecutable("dapr-dashboard", "dapr", ".", "dashboard")
    .WithHttpEndpoint(8080, 8080, "dapr-dashboard", isProxied: false)
    .ExcludeFromManifest();

var catalogApi = builder
    .AddProject<BookWorm_Catalog>(ServiceName.App.Catalog)
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 3500 }))
    .WithReference(catalogDb)
    .WithReference(blobs)
    .WithReference(pubSub)
    .WithReference(keycloak)
    .WaitFor(blobs)
    .WaitFor(catalogDb)
    .WaitFor(keycloak);

var basketApi = builder
    .AddProject<BookWorm_Basket>(ServiceName.App.Basket)
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 3600 }))
    .WithReference(pubSub)
    .WithReference(keycloak)
    .WithReference(stateStore)
    .WaitFor(keycloak);

var orderingApi = builder
    .AddProject<BookWorm_Ordering>(ServiceName.App.Ordering)
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 3700 }))
    .WithReference(orderingDb)
    .WithReference(pubSub)
    .WithReference(keycloak)
    .WaitFor(orderingDb)
    .WaitFor(keycloak);

var ratingApi = builder
    .AddProject<BookWorm_Rating>(ServiceName.App.Rating)
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 3800 }))
    .WithReference(ratingDb)
    .WithReference(pubSub)
    .WithReference(keycloak)
    .WaitFor(ratingDb)
    .WaitFor(keycloak);

var customerApi = builder
    .AddProject<BookWorm_Customer>(ServiceName.App.Customer)
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 3900 }))
    .WithReference(customerDb)
    .WithReference(pubSub)
    .WithReference(keycloak)
    .WaitFor(customerDb)
    .WaitFor(keycloak);

var inventoryApi = builder
    .AddProject<BookWorm_Inventory>(ServiceName.App.Inventory)
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 4000 }))
    .WithReference(inventoryDb)
    .WithReference(pubSub)
    .WithReference(keycloak)
    .WaitFor(inventoryDb)
    .WaitFor(keycloak);

var paymentApi = builder
    .AddProject<BookWorm_Payment>(ServiceName.App.Payment)
    .WithReference(paymentDb)
    .WithReference(pubSub)
    .WithReference(keycloak)
    .WaitFor(paymentDb)
    .WaitFor(keycloak);

builder
    .AddProject<BookWorm_Notification>(ServiceName.App.Notification)
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 4100 }));

var gateway = builder
    .AddProject<BookWorm_ApiGateway>(ServiceName.App.Gateway)
    .WithReference(catalogApi)
    .WithReference(basketApi)
    .WithReference(orderingApi)
    .WithReference(ratingApi)
    .WithReference(customerApi)
    .WithReference(inventoryApi)
    .WithReference(paymentApi)
    .WaitFor(catalogApi)
    .WaitFor(basketApi)
    .WaitFor(orderingApi)
    .WaitFor(ratingApi)
    .WaitFor(customerApi)
    .WaitFor(inventoryApi)
    .WaitFor(paymentApi);

builder.AddProject<BookWorm_BackOffice>("bookworm-backoffice").WithReference(gateway);

builder.Build().Run();
