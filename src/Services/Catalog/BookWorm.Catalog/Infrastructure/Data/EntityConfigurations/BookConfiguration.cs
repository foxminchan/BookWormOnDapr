using BookWorm.Catalog.Domain.BookAggregate;

namespace BookWorm.Catalog.Infrastructure.Data.EntityConfigurations;

internal sealed class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(p => p.Name).HasMaxLength(DataSchemaLength.Medium).IsRequired();

        builder.Property(p => p.Description).HasMaxLength(DataSchemaLength.SuperLarge).IsRequired();

        builder.Property(p => p.Price).HasPrecision(18, 2).IsRequired();

        builder.Property(p => p.ImageUrl).HasMaxLength(DataSchemaLength.SuperLarge);

        builder.Property(p => p.AverageRating).HasDefaultValue(0.0);

        builder.Property(p => p.TotalReviews).HasDefaultValue(0);

        builder.Property(e => e.CreatedAt).HasDefaultValue(DateTime.UtcNow);

        builder.Property(e => e.LastModifiedAt).HasDefaultValue(DateTime.UtcNow);

        builder.Property(e => e.Version).IsConcurrencyToken();

        builder.HasIndex(e => e.IsDeleted);

        builder.HasMany(x => x.BookAuthors).WithOne(x => x.Book).OnDelete(DeleteBehavior.NoAction);

        builder.Navigation(e => e.BookAuthors).AutoInclude();

        builder
            .HasMany(x => x.BookCategories)
            .WithOne(x => x.Book)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Navigation(e => e.BookCategories).AutoInclude();
    }
}
