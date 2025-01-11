using System.Text.Json;
using BookWorm.ServiceDefaults;
using BookWorm.SharedKernel.ActivityScope;
using BookWorm.SharedKernel.Command;
using BookWorm.SharedKernel.Converters;
using BookWorm.SharedKernel.Endpoints;
using BookWorm.SharedKernel.Exceptions;
using BookWorm.SharedKernel.Pipelines;
using BookWorm.SharedKernel.Query;
using BookWorm.SharedKernel.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Http.Json;

namespace BookWorm.Customer.Extensions;

internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.AddVersioning();

        builder.AddOpenApi();

        builder.Services.AddDaprClient();

        builder.AddEndpoints(typeof(ICustomerApiMarker));

        builder.Services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.Converters.Add(new StringTrimmerJsonConverter());
        });

        builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<ICustomerApiMarker>();
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ActivityBehavior<,>));
            cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        builder.Services.AddValidatorsFromAssemblyContaining<ICustomerApiMarker>(
            includeInternalTypes: true
        );

        builder.Services.AddSingleton<IActivityScope, ActivityScope>();
        builder.Services.AddSingleton<CommandHandlerMetrics>();
        builder.Services.AddSingleton<QueryHandlerMetrics>();
    }
}
