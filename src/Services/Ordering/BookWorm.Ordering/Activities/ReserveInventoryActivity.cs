using Dapr.Workflow;

namespace BookWorm.Ordering.Activities;

internal sealed record InventoryResult(bool IsAvailable, Dictionary<Guid, int> UnavailableItems);

internal sealed class ReserveInventoryActivity : WorkflowActivity<List<Guid>, InventoryResult>
{
    public override Task<InventoryResult> RunAsync(
        WorkflowActivityContext context,
        List<Guid> input
    )
    {
        throw new NotImplementedException();
    }
}
