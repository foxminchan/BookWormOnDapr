using BookWorm.Customer.Domain;

namespace BookWorm.Customer.Infrastructure;

public sealed class CustomerDbContext(DbContextOptions<CustomerDbContext> options)
    : DbContext(options),
        IDatabaseFacade
{
    public DbSet<Consumer> Consumers => Set<Consumer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
    }
}
