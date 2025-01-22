using BookWorm.Inventory.Domain;
using BookWorm.SharedKernel.EF;
using Microsoft.EntityFrameworkCore;

namespace BookWorm.Inventory.Infrastructure;

public sealed class InventoryDbContext(DbContextOptions<InventoryDbContext> options)
    : DbContext(options),
        IDatabaseFacade
{
    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContext).Assembly);
    }
}
