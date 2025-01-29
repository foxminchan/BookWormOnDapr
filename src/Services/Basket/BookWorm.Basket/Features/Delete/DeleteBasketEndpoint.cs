using System.Security.Claims;

namespace BookWorm.Basket.Features.Delete;

internal sealed class DeleteBasketEndpoint
    : IEndpoint<Results<NoContent, NotFound<ProblemDetails>>, ClaimsPrincipal, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/baskets",
                async (ClaimsPrincipal claimsPrincipal, ISender sender) =>
                    await HandleAsync(claimsPrincipal, sender)
            )
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .WithTags(nameof(Basket))
            .MapToApiVersion(new(1, 0))
            .RequireAuthorization();
    }

    public async Task<Results<NoContent, NotFound<ProblemDetails>>> HandleAsync(
        ClaimsPrincipal claimsPrincipal,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var id = Guid.Parse(claimsPrincipal.GetCustomerId());

        var result = await sender.Send(new DeleteBasketCommand(id), cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new() { Detail = $"Basket with id {id} not found." }
            )
            : TypedResults.NoContent();
    }
}
