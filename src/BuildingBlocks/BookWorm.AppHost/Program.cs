using Aspire.Hosting.Dapr;
using BookWor.Constants;
using Microsoft.Extensions.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgresUser = builder.AddParameter("SqlUser", true);
var postgresPassword = builder.AddParameter("SqlPassword", true);
var rabbitUser = builder.AddParameter("RabbitUser");
var rabbitPass = builder.AddParameter("RabbitPassword", true);

var postgres = builder
    .AddPostgres("postgres", postgresUser, postgresPassword, 5432)
    .WithPgWeb()
    .WithDataBindMount("../../../mnt/postgres")
    .WithLifetime(ContainerLifetime.Persistent);

var catalogDb = builder.AddPostgres(ServiceName.Database.Catalog);
var orderingDb = builder.AddPostgres(ServiceName.Database.Ordering);
var ratingDb = builder.AddPostgres(ServiceName.Database.Rating);
var customerDb = builder.AddPostgres(ServiceName.Database.Customer);

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

var stateStore = builder.AddDaprStateStore(ServiceName.Component.Store);

var pubSub = builder
    .AddDaprPubSub(
        ServiceName.Component.Pubsub,
        new DaprComponentOptions { LocalPath = "../../../dapr/components/pubsub.yaml" }
    )
    .WaitFor(rabbitMq);

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

builder
    .AddProject<BookWorm_Catalog>("bookworm-catalog")
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 3500 }))
    .WithReference(catalogDb)
    .WithReference(blobs)
    .WithReference(pubSub)
    .WaitFor(blobs)
    .WaitFor(catalogDb);

builder
    .AddProject<BookWorm_Basket>("bookworm-basket")
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 3600 }))
    .WithReference(pubSub);

builder
    .AddProject<BookWorm_Ordering>("bookworm-ordering")
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 3700 }))
    .WithReference(orderingDb)
    .WithReference(pubSub)
    .WaitFor(orderingDb);

builder
    .AddProject<BookWorm_Rating>("bookworm-rating")
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 3800 }))
    .WithReference(ratingDb)
    .WithReference(pubSub)
    .WaitFor(ratingDb);

builder
    .AddProject<BookWorm_Customer>("bookworm-customer")
    .WithDaprSidecar(o => o.WithOptions(new DaprSidecarOptions { DaprHttpPort = 3900 }))
    .WithReference(customerDb)
    .WithReference(pubSub)
    .WaitFor(customerDb);

builder.Build().Run();
