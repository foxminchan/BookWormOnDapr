using Ardalis.Result;
using BookWorm.Customer.Domain;
using BookWorm.SharedKernel.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookWorm.Customer.Features.Update;

internal sealed class UpdateConsumerInformationEndpoint
    : IEndpoint<
        Results<NoContent, NotFound<ProblemDetails>>,
        UpdateConsumerInformationCommand,
        ISender
    >
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(
                "/consumers/info",
                async (UpdateConsumerInformationCommand command, ISender sender) =>
                    await HandleAsync(command, sender)
            )
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .WithOpenApi()
            .WithTags(nameof(Consumer))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Results<NoContent, NotFound<ProblemDetails>>> HandleAsync(
        UpdateConsumerInformationCommand command,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(command, cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new() { Detail = $"Consumer with id {command.Id} not found." }
            )
            : TypedResults.NoContent();
    }
}
