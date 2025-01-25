using BookWorm.Ordering.Domain;
using BookWorm.Ordering.Features.Create;

namespace BookWorm.Ordering.Activities;

internal sealed class PlaceOrderActivity(ISender sender, ILoggerFactory loggerFactory)
    : WorkflowActivity<Order, Guid>
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<PlaceOrderActivity>();

    public override async Task<Guid> RunAsync(WorkflowActivityContext context, Order input)
    {
        _logger.LogInformation(
            "[{Activity}] - Placing order for {Id}",
            nameof(PlaceOrderActivity),
            input.Id
        );

        var result = await sender.Send(new CreateOrderCommand(input));

        return result.Value;
    }
}
