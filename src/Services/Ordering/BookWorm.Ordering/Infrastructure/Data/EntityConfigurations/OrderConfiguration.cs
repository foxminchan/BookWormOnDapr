using BookWorm.Ordering.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookWorm.Ordering.Infrastructure.Data.EntityConfigurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.No).IsRequired();

        builder.Property(e => e.Notes).HasMaxLength(DataSchemaLength.SuperLarge);

        builder.Property(e => e.CreatedAt).HasDefaultValue(DateTime.UtcNow);

        builder.Property(e => e.LastModifiedAt).HasDefaultValue(DateTime.UtcNow);

        builder.Property(e => e.Version).IsConcurrencyToken();

        builder.HasIndex(e => e.IsDeleted);

        builder
            .HasMany(e => e.Items)
            .WithOne(e => e.Order)
            .HasForeignKey(e => e.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Navigation(e => e.Items)
            .AutoInclude()
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
