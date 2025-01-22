using System.ComponentModel;
using Ardalis.Result;
using BookWorm.Inventory.Domain;
using BookWorm.SharedKernel.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookWorm.Inventory.Features.Warehouses.Get;

internal sealed class GetWarehouseEndpoint
    : IEndpoint<Results<Ok<WareouseDetailDto>, NotFound<ProblemDetails>>, long, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/warehouses/{id:long}",
                async ([Description("The warehouse id")] long id, ISender sender) =>
                    await HandleAsync(id, sender)
            )
            .Produces<WareouseDetailDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .WithTags(nameof(Warehouse))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Results<Ok<WareouseDetailDto>, NotFound<ProblemDetails>>> HandleAsync(
        long id,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(new GetWarehouseQuery(id), cancellationToken);

        return result.Status == ResultStatus.NotFound
            ? TypedResults.NotFound<ProblemDetails>(
                new() { Detail = $"Warehouse with id {id} not found." }
            )
            : TypedResults.Ok(result.Value);
    }
}
