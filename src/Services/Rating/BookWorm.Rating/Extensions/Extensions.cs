using BookWorm.Rating.Infrastructure;

namespace BookWorm.Rating.Extensions;

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

        builder.Services.AddEndpoints(typeof(IRatingApiMarker));

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
            cfg.RegisterServicesFromAssemblyContaining<IRatingApiMarker>();
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ActivityBehavior<,>));
            cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        builder.Services.AddValidatorsFromAssemblyContaining<IRatingApiMarker>(
            includeInternalTypes: true
        );

        builder.Services.AddSingleton<IActivityScope, ActivityScope>();
        builder.Services.AddSingleton<CommandHandlerMetrics>();
        builder.Services.AddSingleton<QueryHandlerMetrics>();

        builder.AddPersistence();
        builder.Services.AddScoped<IEventBus, DaprEventBus>();
    }
}
