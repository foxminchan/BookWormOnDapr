using BookWorm.Catalog.Domain;

namespace BookWorm.Catalog.Features.Categories.List;

internal sealed class ListCategoriesEndpoint : IEndpoint<Ok<IReadOnlyList<CategoryDto>>, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/categories", async (ISender sender) => await HandleAsync(sender))
            .Produces<Ok<IReadOnlyList<CategoryDto>>>()
            .WithOpenApi()
            .WithTags(nameof(Category))
            .MapToApiVersion(new(1, 0));
    }

    public async Task<Ok<IReadOnlyList<CategoryDto>>> HandleAsync(
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var response = await sender.Send(new ListCategoriesQuery(), cancellationToken);

        return TypedResults.Ok(response.Value);
    }
}
