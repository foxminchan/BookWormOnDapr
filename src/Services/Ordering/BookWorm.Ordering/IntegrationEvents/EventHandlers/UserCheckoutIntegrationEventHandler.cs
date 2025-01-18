using BookWorm.Ordering.IntegrationEvents.Events;
using BookWorm.Ordering.Workflows;
using BookWorm.SharedKernel.EventBus.Abstractions;
using Dapr.Workflow;

namespace BookWorm.Ordering.IntegrationEvents.EventHandlers;

public sealed class UserCheckoutIntegrationEventHandler(DaprWorkflowClient daprWorkflowClient)
    : IIntegrationEventHandler<UserCheckedOutIntegrationEvent>
{
    public async Task Handle(UserCheckedOutIntegrationEvent @event)
    {
        var instanceId = $"bookworm-wf-{Guid.CreateVersion7().ToString()[..8]}";

        await daprWorkflowClient.ScheduleNewWorkflowAsync(
            nameof(PlaceOrderWorkflow),
            instanceId,
            @event
        );
    }
}
