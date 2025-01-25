using BookWorm.Catalog.Domain.BookAggregate;
using BookWorm.Catalog.Domain.BookAggregate.Specifications;
using BookWorm.Catalog.Infrastructure.Blob;

namespace BookWorm.Catalog.Features.Books.Update;

internal sealed record UpdateBookCommand(
    Guid Id,
    string Name,
    string? Description,
    IFormFile? Image,
    decimal Price
) : ICommand;

internal sealed class UpdateBookHandler(IRepository<Book> repository, IBlobService blobService)
    : ICommandHandler<UpdateBookCommand>
{
    public async Task<Result> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await repository.FirstOrDefaultAsync(
            new BookFilterSpec(request.Id),
            cancellationToken
        );

        if (book is null)
        {
            return Result.NotFound();
        }

        var imageUrl = book.ImageUrl;

        if (request.Image is not null)
        {
            if (book.ImageUrl is not null)
            {
                await blobService.DeleteFileAsync(book.ImageUrl, cancellationToken);
            }

            imageUrl = await blobService.UploadFileAsync(request.Image, cancellationToken);
        }

        book.Update(request.Name, request.Description, imageUrl, request.Price);

        await repository.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
