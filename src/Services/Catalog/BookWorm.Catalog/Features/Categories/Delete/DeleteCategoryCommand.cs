using BookWorm.Catalog.Domain;

namespace BookWorm.Catalog.Features.Categories.Delete;

internal sealed record DeleteCategoryCommand(Guid Id) : ICommand;

internal sealed class DeleteCategoryHandler(IRepository<Category> repository)
    : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(
        DeleteCategoryCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var category = await repository.GetByIdAsync(command.Id, cancellationToken);

        if (category is null)
        {
            return Result.NotFound();
        }

        await repository.DeleteAsync(category, cancellationToken);

        return Result.NoContent();
    }
}
