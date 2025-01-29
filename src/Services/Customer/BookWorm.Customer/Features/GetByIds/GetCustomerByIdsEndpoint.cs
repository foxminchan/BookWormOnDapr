namespace BookWorm.Customer.Features.GetByIds;

internal sealed class GetCustomerByIdsEndpoint
    : IEndpoint<Ok<Dictionary<Guid, string>>, Guid[], ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/customers/by-ids",
                async ([Description("The customer ids")] Guid[] ids, ISender sender) =>
                    await HandleAsync(ids, sender)
            )
            .Produces<Dictionary<Guid, string>>()
            .WithOpenApi()
            .WithTags(nameof(Customer))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Ok<Dictionary<Guid, string>>> HandleAsync(
        Guid[] ids,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(new GetCustomerByIdsQuery(ids), cancellationToken);

        return TypedResults.Ok(result.Value);
    }
}
