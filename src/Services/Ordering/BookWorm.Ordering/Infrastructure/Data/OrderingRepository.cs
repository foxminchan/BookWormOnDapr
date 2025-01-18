using Ardalis.Specification.EntityFrameworkCore;
using BookWorm.SharedKernel.Core.Model;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Ordering.Infrastructure.Data;

public sealed class OrderingRepository<T>(OrderingDbContext dbContext)
    : RepositoryBase<T>(dbContext),
        IReadRepository<T>,
        IRepository<T>
    where T : class, IAggregateRoot;
