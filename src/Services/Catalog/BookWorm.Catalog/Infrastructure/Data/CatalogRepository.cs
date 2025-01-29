namespace BookWorm.Catalog.Infrastructure.Data;

public sealed class CatalogRepository<T>(CatalogDbContext dbContext)
    : RepositoryBase<T>(dbContext),
        IReadRepository<T>,
        IRepository<T>
    where T : class, IAggregateRoot;
