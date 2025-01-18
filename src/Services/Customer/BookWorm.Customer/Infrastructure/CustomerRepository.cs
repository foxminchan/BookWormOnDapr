using Ardalis.Specification.EntityFrameworkCore;
using BookWorm.SharedKernel.Core.Model;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Customer.Infrastructure;

public sealed class CustomerRepository<T>(CustomerDbContext dbContext)
    : RepositoryBase<T>(dbContext),
        IReadRepository<T>,
        IRepository<T>
    where T : class, IAggregateRoot;
