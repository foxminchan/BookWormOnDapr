using System.Security.Claims;
using BookWorm.Basket.Domain;

namespace BookWorm.Basket.Features.Get;

internal sealed class GetBasketEndpoint
    : IEndpoint<Results<Ok<Card>, NotFound<ProblemDetails>>, ClaimsPrincipal, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/baskets",
                async (ClaimsPrincipal claimsPrincipal, ISender sender) =>
                    await HandleAsync(claimsPrincipal, sender)
            )
            .Produces<Card>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .WithTags(nameof(Basket))
            .MapToApiVersion(new(1, 0))
            .RequireAuthorization();
    }

    public async Task<Results<Ok<Card>, NotFound<ProblemDetails>>> HandleAsync(
        ClaimsPrincipal claimsPrincipal,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var id = Guid.Parse(claimsPrincipal.GetCustomerId());

        var result = await sender.Send(new GetBasketQuery(id), cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new() { Detail = $"Basket with id {id} not found." }
            )
            : TypedResults.Ok(result.Value);
    }
}
