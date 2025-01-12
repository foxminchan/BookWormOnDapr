using Ardalis.Result;
using BookWorm.Catalog.Domain;
using BookWorm.SharedKernel.Command;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Catalog.Features.Categories.Create;

internal sealed record CreateCategoryCommand(string Name) : ICommand<Result<Guid>>;

internal sealed class CreateCategoryHandler(IRepository<Category> repository)
    : ICommandHandler<CreateCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken
    )
    {
        var category = new Category(request.Name);

        await repository.AddAsync(category, cancellationToken);

        return category.Id;
    }
}
