﻿using BookWorm.Ordering.Activities;
using BookWorm.Ordering.Domain;
using BookWorm.Ordering.Features;
using BookWorm.Ordering.Infrastructure.Data;
using BookWorm.Ordering.Infrastructure.EventStore;
using BookWorm.Ordering.IntegrationEvents.EventHandlers;
using BookWorm.Ordering.Workflows;

namespace BookWorm.Ordering.Extensions;

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

        builder.Services.AddEndpoints(typeof(IOrderingApiMarker));

        builder.Services.AddSubscribers(typeof(IOrderingApiMarker));

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
            cfg.RegisterServicesFromAssemblyContaining<IOrderingApiMarker>();
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ActivityBehavior<,>));
            cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        builder.Services.AddValidatorsFromAssemblyContaining<IOrderingApiMarker>(
            includeInternalTypes: true
        );

        builder.Services.AddSingleton<IActivityScope, ActivityScope>();
        builder.Services.AddSingleton<CommandHandlerMetrics>();
        builder.Services.AddSingleton<QueryHandlerMetrics>();

        builder.AddPersistence();

        builder.Services.AddDaprWorkflow(options =>
        {
            // Register the workflow(s) with the Dapr runtime.
            options.RegisterWorkflow<PlaceOrderWorkflow>();
            options.RegisterWorkflow<OrderApprovalSubWorkflow>();

            // These are the activities that get invoked by the workflow(s).
            options.RegisterActivity<GetProductInformationActivity>();
            options.RegisterActivity<NotifyActivity>();
            options.RegisterActivity<PlaceOrderActivity>();
            options.RegisterActivity<RequestApprovalActivity>();
            options.RegisterActivity<ReserveInventoryActivity>();
        });

        builder.Services.AddScoped<IEventBus, DaprEventBus>();
        builder.Services.AddScoped<UserCheckedOutIntegrationEventHandler>();

        builder.AddEventStore(configureOptions: options =>
        {
            options.Projections.LiveStreamAggregation<OrderSummary>();
            options.Projections.Add<Projection>(ProjectionLifecycle.Async);
        });
    }
}
