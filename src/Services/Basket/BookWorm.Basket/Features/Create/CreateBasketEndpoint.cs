using System.Security.Claims;

namespace BookWorm.Basket.Features.Create;

internal sealed class CreateBasketEndpoint
    : IEndpoint<Created<Guid>, CreateBasketCommand, ClaimsPrincipal, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/baskets",
                async (
                    CreateBasketCommand command,
                    ClaimsPrincipal claimsPrincipal,
                    ISender sender
                ) => await HandleAsync(command, claimsPrincipal, sender)
            )
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .WithOpenApi()
            .WithTags(nameof(Basket))
            .MapToApiVersion(new(1, 0))
            .RequireAuthorization();
    }

    public async Task<Created<Guid>> HandleAsync(
        CreateBasketCommand command,
        ClaimsPrincipal claimsPrincipal,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var customerId = Guid.Parse(claimsPrincipal.GetCustomerId());

        var result = await sender.Send(command with { CustomerId = customerId }, cancellationToken);

        return TypedResults.Created(
            new UrlBuilder().WithVersion().WithResource("Baskets").WithId(result.Value).Build(),
            result.Value
        );
    }
}
