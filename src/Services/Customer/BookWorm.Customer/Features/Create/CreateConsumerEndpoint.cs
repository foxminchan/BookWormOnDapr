using BookWorm.Customer.Domain;
using BookWorm.SharedKernel.Core;

namespace BookWorm.Customer.Features.Create;

internal sealed class CreateConsumerEndpoint
    : IEndpoint<Created<Guid>, CreateConsumerCommand, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/consumers",
                async (CreateConsumerCommand command, ISender sender) =>
                    await HandleAsync(command, sender)
            )
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .WithOpenApi()
            .WithTags(nameof(Consumer))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Created<Guid>> HandleAsync(
        CreateConsumerCommand command,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(command, cancellationToken);

        return TypedResults.Created(
            new UrlBuilder().WithVersion().WithResource("Consumers").WithId(result.Value).Build(),
            result.Value
        );
    }
}
