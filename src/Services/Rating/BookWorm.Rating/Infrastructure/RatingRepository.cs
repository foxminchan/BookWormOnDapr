namespace BookWorm.Rating.Infrastructure;

public sealed class RatingRepository<T>(RatingDbContext dbContext)
    : RepositoryBase<T>(dbContext),
        IReadRepository<T>,
        IRepository<T>
    where T : class, IAggregateRoot;
