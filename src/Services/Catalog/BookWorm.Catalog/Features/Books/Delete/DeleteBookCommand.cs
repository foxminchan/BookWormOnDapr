using Ardalis.Result;
using BookWorm.Catalog.Domain.BookAggregate;
using BookWorm.Catalog.Domain.BookAggregate.Specifications;
using BookWorm.SharedKernel.Command;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Catalog.Features.Books.Delete;

internal sealed record DeleteBookCommand(Guid Id) : ICommand;

internal sealed class DeleteBookHandler(IRepository<Book> repository)
    : ICommandHandler<DeleteBookCommand>
{
    public async Task<Result> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await repository.FirstOrDefaultAsync(
            new BookFilterSpec(request.Id),
            cancellationToken
        );

        if (book is null)
        {
            return Result.NotFound();
        }

        book.Delete();

        await repository.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
