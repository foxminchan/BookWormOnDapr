using BookWorm.Catalog.Domain;
using BookWorm.Catalog.Domain.BookAggregate;

namespace BookWorm.Catalog.Infrastructure.Data;

public sealed class CatalogDbContext(DbContextOptions<CatalogDbContext> options)
    : DbContext(options),
        IDatabaseFacade
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<BookAuthor> BookAuthors => Set<BookAuthor>();
    public DbSet<BookCategory> BookCategories => Set<BookCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }
}
