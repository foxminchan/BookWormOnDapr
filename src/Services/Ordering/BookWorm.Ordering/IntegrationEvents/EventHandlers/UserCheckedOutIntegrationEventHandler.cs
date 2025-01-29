using BookWorm.Ordering.IntegrationEvents.Events;
using BookWorm.Ordering.Workflows;

namespace BookWorm.Ordering.IntegrationEvents.EventHandlers;

public sealed class UserCheckedOutIntegrationEventHandler(DaprWorkflowClient daprWorkflowClient)
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
