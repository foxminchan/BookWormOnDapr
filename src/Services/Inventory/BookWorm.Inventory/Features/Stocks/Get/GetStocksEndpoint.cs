﻿using BookWorm.Inventory.Domain;
using BookWorm.SharedKernel.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookWorm.Inventory.Features.Stocks.Get;

internal sealed class GetStocksEndpoint : IEndpoint<Ok<Dictionary<Guid, int>>, Guid[], ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/stocks",
                async (Guid[] productIds, ISender sender) => await HandleAsync(productIds, sender)
            )
            .Produces<Dictionary<Guid, int>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .WithTags(nameof(Stock))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Ok<Dictionary<Guid, int>>> HandleAsync(
        Guid[] productIds,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(new GetStocksQuery(productIds), cancellationToken);

        return TypedResults.Ok(result.Value);
    }
}
