using BookWorm.Ordering.IntegrationEvents.Events;
using BookWorm.SharedKernel.EventBus.Abstractions;
using Dapr.Workflow;

namespace BookWorm.Ordering.Activities;

internal sealed record Notification(string Message, Guid? CustomerId);

internal sealed class NotifyActivity(IEventBus eventBus, ILoggerFactory loggerFactory)
    : WorkflowActivity<Notification, object>
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<NotifyActivity>();

    public override async Task<object> RunAsync(WorkflowActivityContext context, Notification input)
    {
        _logger.LogInformation(
            "[{Activity}] - Sending notification to customer {CustomerId}: {Message}",
            nameof(NotifyActivity),
            input.CustomerId,
            input.Message
        );

        if (input.CustomerId is null)
        {
            _logger.LogWarning(
                "[{Activity}] - CustomerId is null. Cannot send notification.",
                nameof(NotifyActivity)
            );

            return Task.FromResult<object>(default!);
        }

        await eventBus.PublishAsync(
            new NotifySentIntegrationEvent(input.CustomerId, input.Message)
        );

        return Task.FromResult<object>(default!);
    }
}
