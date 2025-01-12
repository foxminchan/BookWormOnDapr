using Ardalis.Result;
using BookWorm.Catalog.Domain;
using BookWorm.SharedKernel.Command;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Catalog.Features.Authors.Create;

internal sealed record CreateAuthorCommand(string Name) : ICommand<Result<Guid>>;

internal sealed record CreateAuthorHandler(IRepository<Author> Repository)
    : ICommandHandler<CreateAuthorCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateAuthorCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var author = new Author(command.Name);

        await Repository.AddAsync(author, cancellationToken);

        return author.Id;
    }
}
