namespace BookWorm.Ordering.Activities;

internal sealed record InventoryResult(bool IsAvailable, Dictionary<Guid, int> UnavailableItems);

internal sealed class ReserveInventoryActivity(DaprClient daprClient, ILoggerFactory loggerFactory)
    : WorkflowActivity<List<Guid>, InventoryResult>
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<ReserveInventoryActivity>();

    public override async Task<InventoryResult> RunAsync(
        WorkflowActivityContext context,
        List<Guid> input
    )
    {
        _logger.LogInformation(
            "[{Activity}] - Reserving inventory for {ProductCount} products",
            nameof(ReserveInventoryActivity),
            input.Count
        );

        var inventory = await daprClient.InvokeMethodAsync<Dictionary<Guid, int>>(
            HttpMethod.Get,
            ServiceName.App.Inventory,
            $"/api/v1/stocks?productIds={string.Join(",", input)}"
        );

        var unavailableItems = inventory
            .Where(x => x.Value <= 0)
            .ToDictionary(x => x.Key, x => x.Value);

        return new(unavailableItems.Count == 0, unavailableItems);
    }
}
