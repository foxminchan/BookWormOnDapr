using Ardalis.Result;
using BookWorm.Catalog.Domain;
using BookWorm.SharedKernel.Query;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Catalog.Features.Categories.List;

internal sealed record ListCategoriesQuery : IQuery<Result<IReadOnlyList<CategoryDto>>>;

internal sealed class ListCategoriesHandler(IReadRepository<Category> repository)
    : IQueryHandler<ListCategoriesQuery, Result<IReadOnlyList<CategoryDto>>>
{
    public async Task<Result<IReadOnlyList<CategoryDto>>> Handle(
        ListCategoriesQuery request,
        CancellationToken cancellationToken
    )
    {
        var categories = await repository.ListAsync(cancellationToken);

        return Result.Success(categories.ToCategoryDtos());
    }
}
