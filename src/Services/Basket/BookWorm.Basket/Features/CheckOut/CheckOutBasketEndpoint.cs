using System.Security.Claims;

namespace BookWorm.Basket.Features.CheckOut;

internal sealed class CheckOutBasketEndpoint
    : IEndpoint<Results<Accepted, NotFound<ProblemDetails>>, ClaimsPrincipal, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/baskets/checkout",
                async (ClaimsPrincipal claimsPrincipal, ISender sender) =>
                    await HandleAsync(claimsPrincipal, sender)
            )
            .Produces(StatusCodes.Status202Accepted)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .WithTags(nameof(Basket))
            .MapToApiVersion(new(1, 0))
            .RequireAuthorization();
    }

    public async Task<Results<Accepted, NotFound<ProblemDetails>>> HandleAsync(
        ClaimsPrincipal claimsPrincipal,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var customerId = Guid.Parse(claimsPrincipal.GetCustomerId());

        var result = await sender.Send(new CheckOutBasketCommand(customerId), cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new() { Detail = $"Basket with id {customerId} not found." }
            )
            : TypedResults.Accepted(
                new UrlBuilder().WithVersion().WithResource("Baskets").WithId(result.Value).Build()
            );
    }
}
