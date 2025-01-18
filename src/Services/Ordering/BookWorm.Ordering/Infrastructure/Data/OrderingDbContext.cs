using Ardalis.GuardClauses;
using BookWorm.Ordering.Domain;
using BookWorm.SharedKernel.EF;
using BookWorm.SharedKernel.Mediator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookWorm.Ordering.Infrastructure.Data;

public sealed class OrderingDbContext(
    DbContextOptions<OrderingDbContext> options,
    IPublisher publisher
) : DbContext(options), IDatabaseFacade
{
    private readonly IPublisher _publisher = Guard.Against.Null(publisher);
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Item> Items => Set<Item>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderingDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        await _publisher.DispatchDomainEventsAsync(this);

        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        _publisher.DispatchDomainEventsAsync(this).GetAwaiter().GetResult();

        return base.SaveChanges();
    }
}
