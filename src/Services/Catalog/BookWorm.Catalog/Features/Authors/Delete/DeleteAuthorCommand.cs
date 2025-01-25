using BookWorm.Catalog.Domain;

namespace BookWorm.Catalog.Features.Authors.Delete;

internal sealed record DeleteAuthorCommand(Guid Id) : ICommand;

internal sealed class DeleteAuthorHandler(IRepository<Author> repository)
    : ICommandHandler<DeleteAuthorCommand>
{
    public async Task<Result> Handle(
        DeleteAuthorCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var author = await repository.GetByIdAsync(command.Id, cancellationToken);

        if (author is null)
        {
            return Result.NotFound();
        }

        await repository.DeleteAsync(author, cancellationToken);

        return Result.NoContent();
    }
}
