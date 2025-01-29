using System.Security.Claims;
using BookWorm.Rating.Domain;

namespace BookWorm.Rating.Features.Create;

internal sealed class CreateRatingEndpoint
    : IEndpoint<Created<Guid>, CreateRatingCommand, ClaimsPrincipal, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/reviews",
                async (
                    CreateRatingCommand command,
                    ClaimsPrincipal claimsPrincipal,
                    ISender sender
                ) => await HandleAsync(command, claimsPrincipal, sender)
            )
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .WithOpenApi()
            .WithTags(nameof(Review))
            .MapToApiVersion(new(1, 0))
            .RequireAuthorization();
    }

    public async Task<Created<Guid>> HandleAsync(
        CreateRatingCommand command,
        ClaimsPrincipal claimsPrincipal,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var customerId = Guid.Parse(claimsPrincipal.GetCustomerId());

        var result = await sender.Send(command with { CustomerId = customerId }, cancellationToken);

        return TypedResults.Created(
            new UrlBuilder().WithVersion().WithResource("Reviews").WithId(result.Value).Build(),
            result.Value
        );
    }
}
