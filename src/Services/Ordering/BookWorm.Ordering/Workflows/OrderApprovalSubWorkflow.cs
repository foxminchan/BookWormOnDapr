using BookWorm.Ordering.Activities;
using BookWorm.Ordering.Contracts;
using BookWorm.Ordering.IntegrationEvents.Events;
using Dapr.Workflow;

namespace BookWorm.Ordering.Workflows;

internal sealed class OrderApprovalSubWorkflow : Workflow<ApprovalRequest, bool>
{
    private readonly WorkflowTaskOptions retryOptions = new()
    {
        RetryPolicy = new WorkflowRetryPolicy(
            backoffCoefficient: 2.0,
            maxRetryInterval: TimeSpan.FromHours(1),
            maxNumberOfAttempts: 10,
            firstRetryInterval: TimeSpan.FromSeconds(5)
        ),
    };

    public override async Task<bool> RunAsync(WorkflowContext context, ApprovalRequest input)
    {
        const decimal Threshold = 15000.0m;

        if (input.Total >= Threshold)
        {
            context.SetCustomStatus("Requesting approval for the order");

            // Request approval for the order from the manager
            await context.CallActivityAsync(nameof(RequestApprovalActivity), input, retryOptions);

            try
            {
                // Wait for approval
                context.SetCustomStatus("Waiting for approval...");
                var result =
                    await context.WaitForExternalEventAsync<AdminApprovalResultedIntegrationEvent>(
                        nameof(AdminApprovalResultedIntegrationEvent),
                        TimeSpan.FromMinutes(15)
                    );

                context.SetCustomStatus($"Approval result: {result.ApprovalResult}");

                if (result.ApprovalResult == Approval.Rejected)
                {
                    context.SetCustomStatus("Approval was denied");

                    // Notify user about approval denial
                    await context.CallActivityAsync(
                        nameof(NotifyActivity),
                        new Notification(
                            "Approval denied by the manager, please contact them for more information",
                            input.CustomerId
                        )
                    );

                    return false;
                }
                else if (result.ApprovalResult == Approval.Unspecified)
                {
                    context.SetCustomStatus("Approval was not received");

                    // Notify user about approval denial
                    await context.CallActivityAsync(
                        nameof(NotifyActivity),
                        new Notification(
                            "Approval was not received in time, please try again later",
                            input.CustomerId
                        )
                    );

                    return false;
                }
            }
            catch (TimeoutException)
            {
                context.SetCustomStatus("Due to timeout, the approval was not received");

                // Notify user about approval timeout
                await context.CallActivityAsync(
                    nameof(NotifyActivity),
                    new Notification(
                        "Cancelling order because it didn't receive an approval",
                        input.CustomerId
                    )
                );

                return false;
            }
        }

        return true;
    }
}
