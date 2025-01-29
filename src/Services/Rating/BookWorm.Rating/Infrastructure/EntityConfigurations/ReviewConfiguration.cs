using BookWorm.Rating.Domain;

namespace BookWorm.Rating.Infrastructure.EntityConfigurations;

internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Rating).IsRequired();

        builder.Property(e => e.Comment).HasMaxLength(DataSchemaLength.Max);

        builder.Property(e => e.BookId).IsRequired();

        builder.Property(e => e.CustomerId).IsRequired();

        builder.Property(e => e.CreatedAt).HasDefaultValue(DateTime.UtcNow);

        builder.Property(e => e.LastModifiedAt).HasDefaultValue(DateTime.UtcNow);

        builder.Property(e => e.Version).IsConcurrencyToken();
    }
}
