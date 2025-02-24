﻿using BookWorm.Catalog.Infrastructure.Blob;
using BookWorm.Catalog.Infrastructure.Data;
using BookWorm.Catalog.IntegrationEvents.EventHandlers;

namespace BookWorm.Catalog.Extensions;

internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDaprClient();

        builder.AddServiceDefaults(checksBuilder =>
        {
            checksBuilder.AddDaprHealthCheck();
        });

        builder.AddDefaultAuthentication();

        builder.AddVersioning();

        builder.AddOpenApi();

        builder.Services.AddEndpoints(typeof(ICatalogApiMarker));

        builder.Services.AddSingleton(
            new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                Converters = { new StringTrimmerJsonConverter() },
            }
        );

        builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<ICatalogApiMarker>();
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ActivityBehavior<,>));
            cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        builder.Services.AddValidatorsFromAssemblyContaining<ICatalogApiMarker>(
            includeInternalTypes: true
        );

        builder.Services.AddSingleton<IActivityScope, ActivityScope>();
        builder.Services.AddSingleton<CommandHandlerMetrics>();
        builder.Services.AddSingleton<QueryHandlerMetrics>();
        builder.Services.AddSingleton<IBlobService, BlobService>();

        builder.AddPersistence();

        builder.AddAzureBlobClient(ServiceName.Blob);

        builder.Services.AddScoped<IEventBus, DaprEventBus>();
        builder.Services.AddScoped<RatingAddedIntegrationEventHandler>();
        builder.Services.AddScoped<RatingDeletedIntegrationEventHandler>();
    }
}
