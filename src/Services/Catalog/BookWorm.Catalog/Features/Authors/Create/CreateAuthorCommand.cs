using BookWorm.Catalog.Domain;

namespace BookWorm.Catalog.Features.Authors.Create;

internal sealed record CreateAuthorCommand(string Name) : ICommand<Result<Guid>>;

internal sealed class CreateAuthorHandler(IRepository<Author> repository)
    : ICommandHandler<CreateAuthorCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateAuthorCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var author = new Author(command.Name);

        await repository.AddAsync(author, cancellationToken);

        return author.Id;
    }
}
