using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BookWorm.SharedKernel.Endpoints;

public static class Extensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Type type)
    {
        ServiceDescriptor[] serviceDescriptors = type
            .Assembly.DefinedTypes.Where(type =>
                type is { IsAbstract: false, IsInterface: false }
                && type.IsAssignableTo(typeof(IEndpoint))
            )
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        ApiVersionSet apiVersionSet
    )
    {
        var scope = app.Services.CreateScope();

        var endpoints = scope.ServiceProvider.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = app.MapGroup("/api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }

    public static IServiceCollection AddSubscribers(this IServiceCollection services, Type type)
    {
        ServiceDescriptor[] serviceDescriptors = type
            .Assembly.DefinedTypes.Where(type =>
                type is { IsAbstract: false, IsInterface: false }
                && type.IsAssignableTo(typeof(ISubscriber))
            )
            .Select(type => ServiceDescriptor.Transient(typeof(ISubscriber), type))
            .ToArray();
        services.TryAddEnumerable(serviceDescriptors);
        return services;
    }

    public static IApplicationBuilder MapIntegrationEvents(this WebApplication app)
    {
        var scope = app.Services.CreateScope();

        var subscribers = scope.ServiceProvider.GetRequiredService<IEnumerable<ISubscriber>>();

        foreach (var subscriber in subscribers)
        {
            subscriber.MapIntegrationEventEndpoint(app);
        }

        return app;
    }
}
