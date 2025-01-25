using BookWorm.Ordering.Activities;
using BookWorm.Ordering.Contracts;
using BookWorm.Ordering.Domain;
using BookWorm.Ordering.IntegrationEvents.Events;

namespace BookWorm.Ordering.Workflows;

internal sealed record PlaceOrderWorkflowResult(bool IsSuccess, string? ErrorMessage);

internal sealed class PlaceOrderWorkflow
    : Workflow<UserCheckedOutIntegrationEvent, PlaceOrderWorkflowResult>
{
    private readonly WorkflowTaskOptions _retryOptions = new()
    {
        RetryPolicy = new(
            backoffCoefficient: 2.0,
            maxRetryInterval: TimeSpan.FromHours(1),
            maxNumberOfAttempts: 10,
            firstRetryInterval: TimeSpan.FromSeconds(5)
        ),
    };

    public override async Task<PlaceOrderWorkflowResult> RunAsync(
        WorkflowContext context,
        UserCheckedOutIntegrationEvent input
    )
    {
        context.SetCustomStatus("Retrieving product details and verifying inventory...");

        // Retrieve product details for the items in the order
        var productDetailsTask = context.CallActivityAsync<Dictionary<Guid, decimal>>(
            nameof(GetProductInformationActivity),
            input.Items.Select(i => i.Id).ToList(),
            _retryOptions
        );

        // Determine if there is enough of the item available for purchase by checking the inventory
        var inventoryTask = context.CallActivityAsync<InventoryResult>(
            nameof(ReserveInventoryActivity),
            input.Items.Select(i => i.Id).ToList(),
            _retryOptions
        );

        // Fan-out to retrieve product details and inventory
        await Task.WhenAll(productDetailsTask, inventoryTask);

        // Fan-in to get the results
        var products = await productDetailsTask;
        var inventory = await inventoryTask;

        // Check if inventory is available
        if (!inventory.IsAvailable)
        {
            context.SetCustomStatus("Inventory not available");

            // Notify user about insufficient inventory
            await context.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification("Insufficient inventory for your order", input.CustomerId)
            );

            return new(false, "Inventory not available");
        }

        var order = new Order(
            null,
            input.CustomerId,
            input.Items.Select(i => new Item(i.Id, i.Quantity, products[i.Id])).ToList()
        );

        var total = input.Items.Sum(i => i.Quantity * products[i.Id]);

        // Check if the total exceeds the threshold for approval
        var isApproval = await context.CallChildWorkflowAsync<bool>(
            nameof(OrderApprovalSubWorkflow),
            new ApprovalRequest(order.Id, total, input.CustomerId),
            new($"{context.InstanceId}-sub")
        );

        if (!isApproval)
        {
            return new(false, "Approval denied or not received");
        }

        context.SetCustomStatus("Placing the order...");

        // Store the order information
        await context.CallActivityAsync<Guid>(nameof(PlaceOrderActivity), order, _retryOptions);

        // Notify user about successful order placement
        await context.CallActivityAsync(
            nameof(NotifyActivity),
            new Notification($"Order with id {order.Id} placed successfully", input.CustomerId)
        );

        return new(true, null);
    }
}
