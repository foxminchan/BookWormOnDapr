using Ardalis.Result;
using BookWorm.Catalog.Domain;
using BookWorm.SharedKernel.Query;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Catalog.Features.Authors.List;

internal sealed record ListAuthorsQuery : IQuery<Result<IReadOnlyList<AuthorDto>>>;

internal sealed record ListAuthorsHandler(IReadRepository<Author> repository)
    : IQueryHandler<ListAuthorsQuery, Result<IReadOnlyList<AuthorDto>>>
{
    public async Task<Result<IReadOnlyList<AuthorDto>>> Handle(
        ListAuthorsQuery request,
        CancellationToken cancellationToken
    )
    {
        var authors = await repository.ListAsync(cancellationToken);

        return Result.Success(authors.ToAuthorDtos());
    }
}
