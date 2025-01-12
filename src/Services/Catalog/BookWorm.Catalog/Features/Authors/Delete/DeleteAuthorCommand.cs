using Ardalis.Result;
using BookWorm.Catalog.Domain;
using BookWorm.SharedKernel.Command;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Catalog.Features.Authors.Delete;

internal sealed record DeleteAuthorCommand(Guid Id) : ICommand;

internal sealed class DeleteAuthorHandler(IRepository<Author> Repository)
    : ICommandHandler<DeleteAuthorCommand>
{
    public async Task<Result> Handle(
        DeleteAuthorCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var author = await Repository.GetByIdAsync(command.Id, cancellationToken);

        if (author is null)
        {
            return Result.NotFound();
        }

        await Repository.DeleteAsync(author, cancellationToken);

        return Result.NoContent();
    }
}
