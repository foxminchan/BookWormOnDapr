namespace BookWorm.Basket.Features.Update;

internal sealed class UpdateBasketEndpoint : IEndpoint<NoContent, UpdateBasketCommand, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/basket",
                async (UpdateBasketCommand command, ISender sender) =>
                    await HandleAsync(command, sender)
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
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        await sender.Send(command, cancellationToken);

        return TypedResults.NoContent();
    }
}
