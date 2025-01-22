using Ardalis.Specification.EntityFrameworkCore;
using BookWorm.SharedKernel.Core.Model;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Inventory.Infrastructure;

public sealed class InventoryRepository<T>(InventoryDbContext dbContext)
    : RepositoryBase<T>(dbContext),
        IReadRepository<T>,
        IRepository<T>
    where T : class, IAggregateRoot;
