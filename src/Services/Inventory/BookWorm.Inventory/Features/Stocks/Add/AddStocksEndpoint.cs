using Ardalis.Result;
using BookWorm.Inventory.Domain;
using BookWorm.SharedKernel.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookWorm.Inventory.Features.Stocks.Add;

internal sealed class AddStocksEndpoint
    : IEndpoint<Results<NoContent, NotFound<ProblemDetails>>, AddStocksCommand, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/stocks",
                async (AddStocksCommand command, ISender sender) =>
                    await HandleAsync(command, sender)
            )
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .WithOpenApi()
            .WithTags(nameof(Stock))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Results<NoContent, NotFound<ProblemDetails>>> HandleAsync(
        AddStocksCommand command,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(command, cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new() { Detail = $"Warehouse with id {command.WarehouseId} not found." }
            )
            : TypedResults.NoContent();
    }
}
