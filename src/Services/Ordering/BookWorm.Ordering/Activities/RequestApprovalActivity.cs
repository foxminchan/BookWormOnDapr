using BookWorm.Ordering.Contracts;
using BookWorm.Ordering.IntegrationEvents.Events;

namespace BookWorm.Ordering.Activities;

internal sealed class RequestApprovalActivity(IEventBus eventBus, ILoggerFactory loggerFactory)
    : WorkflowActivity<ApprovalRequest, object>
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<RequestApprovalActivity>();

    public override async Task<object> RunAsync(
        WorkflowActivityContext context,
        ApprovalRequest input
    )
    {
        _logger.LogInformation(
            "[{Activity}] - Requesting approval for order {OrderId}",
            nameof(RequestApprovalActivity),
            input.OrderId
        );

        await eventBus.PublishAsync(
            new RequestedApprovalIntegrationEvent(input.OrderId, input.Total)
        );

        return Task.FromResult<object?>(null);
    }
}
