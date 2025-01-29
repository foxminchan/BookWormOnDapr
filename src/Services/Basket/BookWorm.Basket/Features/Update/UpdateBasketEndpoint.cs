using System.Security.Claims;

namespace BookWorm.Basket.Features.Update;

internal sealed class UpdateBasketEndpoint
    : IEndpoint<NoContent, UpdateBasketCommand, ClaimsPrincipal, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/basket",
                async (
                    UpdateBasketCommand command,
                    ClaimsPrincipal claimsPrincipal,
                    ISender sender
                ) => await HandleAsync(command, claimsPrincipal, sender)
            )
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .DisableAntiforgery()
            .WithOpenApi()
            .WithTags(nameof(Basket))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<NoContent> HandleAsync(
        UpdateBasketCommand command,
        ClaimsPrincipal claimsPrincipal,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var customerId = Guid.Parse(claimsPrincipal.GetCustomerId());

        await sender.Send(command with { CustomerId = customerId }, cancellationToken);

        return TypedResults.NoContent();
    }
}
