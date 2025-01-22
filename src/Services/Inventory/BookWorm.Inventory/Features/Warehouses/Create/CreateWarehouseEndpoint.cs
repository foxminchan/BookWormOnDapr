using BookWorm.Inventory.Domain;
using BookWorm.SharedKernel.Endpoints;
using BookWorm.SharedKernel.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookWorm.Inventory.Features.Warehouses.Create;

internal sealed class CreateWarehouseEndpoint
    : IEndpoint<Created<long>, CreateWarehouseCommand, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/warehouses",
                async (CreateWarehouseCommand command, ISender sender) =>
                    await HandleAsync(command, sender)
            )
            .Produces<long>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .WithOpenApi()
            .WithTags(nameof(Warehouse))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Created<long>> HandleAsync(
        CreateWarehouseCommand command,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(command, cancellationToken);

        return TypedResults.Created(
            new UrlBuilder()
                .WithVersion()
                .WithResource(nameof(Warehouses))
                .WithId(result.Value)
                .Build(),
            result.Value
        );
    }
}
