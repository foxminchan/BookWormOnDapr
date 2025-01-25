using BookWorm.Inventory.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookWorm.Inventory.Infrastructure.EntityConfigurations;

internal sealed class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Location).HasMaxLength(DataSchemaLength.ExtraLarge).IsRequired();

        builder.Property(x => x.Description).HasMaxLength(DataSchemaLength.Max);

        builder.Property(x => x.Website).HasMaxLength(DataSchemaLength.SuperLarge);

        builder.Property(e => e.CreatedAt).HasDefaultValue(DateTime.UtcNow);

        builder.Property(e => e.LastModifiedAt).HasDefaultValue(DateTime.UtcNow);

        builder.Property(e => e.Version).IsConcurrencyToken();

        builder.HasIndex(e => e.IsDeleted);

        builder
            .HasMany(x => x.Stocks)
            .WithOne()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Navigation(e => e.Stocks)
            .AutoInclude()
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
