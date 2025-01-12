using BookWorm.Catalog.Domain.BookAggregate;
using BookWorm.SharedKernel.Endpoints;
using BookWorm.SharedKernel.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookWorm.Catalog.Features.Books.Create;

internal sealed class CreateBookEndpoint : IEndpoint<Created<Guid>, CreateBookCommand, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/books",
                async ([FromForm] CreateBookCommand command, ISender sender) =>
                    await HandleAsync(command, sender)
            )
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .DisableAntiforgery()
            .WithOpenApi()
            .WithTags(nameof(Book))
            .MapToApiVersion(new(1, 0))
            .WithFormOptions(bufferBody: true);
    }

    public async Task<Created<Guid>> HandleAsync(
        CreateBookCommand command,
        ISender sender,
        CancellationToken cancellationToken = default
    )
    {
        var result = await sender.Send(command, cancellationToken);

        return TypedResults.Created(
            new UrlBuilder().WithVersion().WithResource(nameof(Books)).WithId(result.Value).Build(),
            result.Value
        );
    }
}
