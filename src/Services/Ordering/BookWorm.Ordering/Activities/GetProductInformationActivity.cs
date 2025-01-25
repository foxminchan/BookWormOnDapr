namespace BookWorm.Ordering.Activities;

internal sealed class GetProductInformationActivity
    : WorkflowActivity<List<Guid>, Dictionary<Guid, decimal>>
{
    public override Task<Dictionary<Guid, decimal>> RunAsync(
        WorkflowActivityContext context,
        List<Guid> input
    )
    {
        throw new NotImplementedException();
    }
}
