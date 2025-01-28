using Dapr.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BookWorm.ServiceDefaults;

public sealed class DaprHealthCheck(DaprClient daprClient) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default
    )
    {
        var healthy = await daprClient.CheckHealthAsync(cancellationToken);

        return healthy
            ? HealthCheckResult.Healthy("Dapr sidecar is healthy.")
            : new(context.Registration.FailureStatus, "Dapr sidecar is unhealthy.");
    }
}

public static class DaprHealthCheckExtensions
{
    public static IHealthChecksBuilder AddDaprHealthCheck(this IHealthChecksBuilder builder) 
        => builder.AddCheck<DaprHealthCheck>(nameof(Dapr));
}
