using BookWorm.Rating.Domain;

namespace BookWorm.Rating.Infrastructure;

public sealed class RatingDbContext(DbContextOptions<RatingDbContext> options, IPublisher publisher)
    : DbContext(options),
        IDatabaseFacade
{
    private readonly IPublisher _publisher = Guard.Against.Null(publisher);
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RatingDbContext).Assembly);
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
