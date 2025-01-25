using Ardalis.Specification.EntityFrameworkCore;

namespace BookWorm.Inventory.Infrastructure;

public sealed class InventoryRepository<T>(InventoryDbContext dbContext)
    : RepositoryBase<T>(dbContext),
        IReadRepository<T>,
        IRepository<T>
    where T : class, IAggregateRoot;
