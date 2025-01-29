using Aspire.Hosting.Dapr;
using BookWorm.AppHost.Resources;
using BookWorm.Constants;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgresUser = builder.AddParameter("SqlUser", true);
var postgresPassword = builder.AddParameter("SqlPassword", true);

const string appProtocol = "https";
var launchProfileName = builder.Configuration["DOTNET_LAUNCH_PROFILE"] ?? appProtocol;

const string baseDir = "../../..";

var postgres = builder
    .AddPostgres("postgres", postgresUser, postgresPassword, 5432)
    .WithPgAdmin()
    .WithDataBindMount($"{baseDir}/mnt/postgres")
    .WithLifetime(ContainerLifetime.Persistent);

var catalogDb = postgres.AddDatabase(ServiceName.Database.Catalog);
var orderingDb = postgres.AddDatabase(ServiceName.Database.Ordering);
var ratingDb = postgres.AddDatabase(ServiceName.Database.Rating);
var customerDb = postgres.AddDatabase(ServiceName.Database.Customer);
var inventoryDb = postgres.AddDatabase(ServiceName.Database.Inventory);
var paymentDb = postgres.AddDatabase(ServiceName.Database.Payment);
var identityDb = postgres.AddDatabase(ServiceName.Database.Identity);

var storage = builder.AddAzureStorage("storage");

if (builder.Environment.IsDevelopment())
{
    storage.RunAsEmulator(config =>
        config
            .WithDataBindMount($"{baseDir}/mnt/azurite")
            .WithLifetime(ContainerLifetime.Persistent)
    );
}

var blobs = storage.AddBlobs(ServiceName.Blob);

var kafka = builder
    .AddKafka(ServiceName.Bus, 9092)
    .WithKafkaUI()
    .WithDataBindMount($"{baseDir}/mnt/kafka")
    .WithLifetime(ContainerLifetime.Persistent);

var stateStore = builder.AddDaprStateStore(
    ServiceName.Component.Store,
    new() { LocalPath = $"{baseDir}/dapr/components/statestore.yaml" }
);

var pubSub = builder
    .AddDaprPubSub(
        ServiceName.Component.Pubsub,
        new() { LocalPath = $"{baseDir}/dapr/components/pubsub.yaml" }
    )
    .WaitFor(kafka);

var daprOptions = new DaprSidecarOptions
{
    AppProtocol = appProtocol,
    LogLevel = nameof(LogLevel.Debug).ToLowerInvariant(),
    Config = Path.Combine(
        Directory.GetCurrentDirectory(),
        $"{baseDir}/dapr/configuration/config.yaml"
    ),
};

builder
    .AddExecutable("dapr-dashboard", "dapr", ".", "dashboard")
    .WithHttpEndpoint(8080, 8080, "dapr-dashboard", isProxied: false)
    .ExcludeFromManifest();

var identityApi = builder
    .AddProject<BookWorm_Identity>(ServiceName.App.Identity)
    .WithDaprSidecar(o => o.WithOptions(daprOptions with { DaprHttpPort = 3500 }))
    .WithReference(identityDb)
    .WithReference(pubSub)
    .WaitFor(identityDb);

const string identityUrl = "Identity__Url";
var identityEndpoint = identityApi.GetEndpoint(launchProfileName);

var catalogApi = builder
    .AddProject<BookWorm_Catalog>(ServiceName.App.Catalog)
    .WithDaprSidecar(o => o.WithOptions(daprOptions with { DaprHttpPort = 3600 }))
    .WithReference(catalogDb)
    .WithReference(blobs)
    .WithReference(pubSub)
    .WaitFor(blobs)
    .WaitFor(catalogDb)
    .WithEnvironment(identityUrl, identityEndpoint)
    .WaitFor(identityApi);

var basketApi = builder
    .AddProject<BookWorm_Basket>(ServiceName.App.Basket)
    .WithDaprSidecar(o => o.WithOptions(daprOptions with { DaprHttpPort = 3700 }))
    .WithReference(pubSub)
    .WithReference(stateStore)
    .WithEnvironment(identityUrl, identityEndpoint)
    .WaitFor(identityApi);

var orderingApi = builder
    .AddProject<BookWorm_Ordering>(ServiceName.App.Ordering)
    .WithDaprSidecar(o => o.WithOptions(daprOptions with { DaprHttpPort = 3800 }))
    .WithReference(orderingDb)
    .WithReference(pubSub)
    .WaitFor(orderingDb)
    .WithEnvironment(identityUrl, identityEndpoint)
    .WaitFor(identityApi);

var ratingApi = builder
    .AddProject<BookWorm_Rating>(ServiceName.App.Rating)
    .WithDaprSidecar(o => o.WithOptions(daprOptions with { DaprHttpPort = 3900 }))
    .WithReference(ratingDb)
    .WithReference(pubSub)
    .WaitFor(ratingDb)
    .WithEnvironment(identityUrl, identityEndpoint)
    .WaitFor(identityApi);

var customerApi = builder
    .AddProject<BookWorm_Customer>(ServiceName.App.Customer)
    .WithDaprSidecar(o => o.WithOptions(daprOptions with { DaprHttpPort = 4000 }))
    .WithReference(customerDb)
    .WithReference(pubSub)
    .WaitFor(customerDb)
    .WithEnvironment(identityUrl, identityEndpoint)
    .WaitFor(identityApi);

var inventoryApi = builder
    .AddProject<BookWorm_Inventory>(ServiceName.App.Inventory)
    .WithDaprSidecar(o => o.WithOptions(daprOptions with { DaprHttpPort = 4100 }))
    .WithReference(inventoryDb)
    .WithReference(pubSub)
    .WaitFor(inventoryDb)
    .WithEnvironment(identityUrl, identityEndpoint)
    .WaitFor(identityApi);

var paymentApi = builder
    .AddProject<BookWorm_Payment>(ServiceName.App.Payment)
    .WithDaprSidecar(o => o.WithOptions(daprOptions with { DaprHttpPort = 4200 }))
    .WithReference(paymentDb)
    .WithReference(pubSub)
    .WaitFor(paymentDb);

var notificationApi = builder
    .AddProject<BookWorm_Notification>(ServiceName.App.Notification)
    .WithDaprSidecar(o => o.WithOptions(daprOptions with { DaprHttpPort = 4300 }));

var gateway = builder
    .AddProject<BookWorm_ApiGateway>(ServiceName.App.Gateway)
    .WithReference(catalogApi)
    .WithReference(basketApi)
    .WithReference(orderingApi)
    .WithReference(ratingApi)
    .WithReference(customerApi)
    .WithReference(inventoryApi)
    .WithReference(paymentApi)
    .WithReference(identityApi)
    .WaitFor(catalogApi)
    .WaitFor(basketApi)
    .WaitFor(orderingApi)
    .WaitFor(ratingApi)
    .WaitFor(customerApi)
    .WaitFor(inventoryApi)
    .WaitFor(paymentApi)
    .WaitFor(identityApi);

identityApi
    .WithEnvironment("Services__Catalog", catalogApi.GetEndpoint(launchProfileName))
    .WithEnvironment("Services__Ordering", orderingApi.GetEndpoint(launchProfileName))
    .WithEnvironment("Services__Basket", basketApi.GetEndpoint(launchProfileName))
    .WithEnvironment("Services__Rating", ratingApi.GetEndpoint(launchProfileName))
    .WithEnvironment("Services__Customer", customerApi.GetEndpoint(launchProfileName))
    .WithEnvironment("Services__Inventory", inventoryApi.GetEndpoint(launchProfileName))
    .WithEnvironment("Services__Bff", gateway.GetEndpoint(launchProfileName));

var backOffice = builder
    .AddProject<BookWorm_BackOffice>("bookworm-backoffice")
    .WithReference(gateway)
    .WaitFor(gateway);

// Health checks
builder
    .AddHealthChecksUi("healthchecksui")
    .WithReference(gateway)
    .WithReference(identityApi)
    .WithReference(catalogApi)
    .WithReference(orderingApi)
    .WithReference(ratingApi)
    .WithReference(basketApi)
    .WithReference(notificationApi)
    .WithReference(customerApi)
    .WithReference(inventoryApi)
    .WithReference(paymentApi)
    .WithReference(backOffice)
    .WithExternalHttpEndpoints();

builder.Build().Run();
