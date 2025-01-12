using Ardalis.Result;
using BookWorm.Catalog.Domain.BookAggregate;
using BookWorm.Catalog.Infrastructure.Blob;
using BookWorm.SharedKernel.Command;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Catalog.Features.Books.Create;

internal sealed record CreateBookCommand(
    string Name,
    string? Description,
    IFormFile? Image,
    decimal Price,
    Guid[] AuthorIds,
    Guid[] CategoryIds
) : ICommand<Result<Guid>>;

internal sealed class CreateBookHandler(IRepository<Book> repository, IBlobService blobService)
    : ICommandHandler<CreateBookCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateBookCommand request,
        CancellationToken cancellationToken
    )
    {
        string? imageUrl = null;

        if (request.Image is not null)
        {
            imageUrl = await blobService.UploadFileAsync(request.Image, cancellationToken);
        }

        var book = new Book(
            request.Name,
            request.Description,
            imageUrl,
            request.Price,
            request.AuthorIds,
            request.CategoryIds
        );

        var result = await repository.AddAsync(book, cancellationToken);

        return result.Id;
    }
}
