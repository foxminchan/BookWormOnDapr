using System.Text.Json;
using BookWorm.Basket.Domain;
using BookWorm.Basket.IntegrationEvents.EventHandlers;
using BookWorm.Basket.Repositories;
using BookWorm.ServiceDefaults;
using BookWorm.SharedKernel.ActivityScope;
using BookWorm.SharedKernel.Command;
using BookWorm.SharedKernel.Converters;
using BookWorm.SharedKernel.Endpoints;
using BookWorm.SharedKernel.EventBus;
using BookWorm.SharedKernel.EventBus.Abstractions;
using BookWorm.SharedKernel.Exceptions;
using BookWorm.SharedKernel.Pipelines;
using BookWorm.SharedKernel.Query;
using BookWorm.SharedKernel.Versioning;
using FluentValidation;

namespace BookWorm.Basket.Extensions;

internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.AddVersioning();

        builder.AddOpenApi();

        builder.Services.AddDaprClient();

        builder.Services.AddEndpoints(typeof(IBasketApiMarker));

        builder.Services.AddSubscribers(typeof(IBasketApiMarker));

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
            cfg.RegisterServicesFromAssemblyContaining<IBasketApiMarker>();
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ActivityBehavior<,>));
        });

        builder.Services.AddValidatorsFromAssemblyContaining<IBasketApiMarker>(
            includeInternalTypes: true
        );

        builder.Services.AddSingleton<IActivityScope, ActivityScope>();
        builder.Services.AddSingleton<CommandHandlerMetrics>();
        builder.Services.AddSingleton<QueryHandlerMetrics>();

        builder.Services.AddScoped<IEventBus, DaprEventBus>();
        builder.Services.AddScoped<IBasketRepository, DaprBasketRepository>();
        builder.Services.AddScoped<OrderStatusChangedToNewIntegrationEventHandler>();
    }
}
