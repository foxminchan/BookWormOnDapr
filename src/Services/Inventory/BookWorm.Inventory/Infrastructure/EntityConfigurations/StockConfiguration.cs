using BookWorm.Inventory.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookWorm.Inventory.Infrastructure.EntityConfigurations;

internal sealed class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Quantity).IsRequired();

        builder.Property(e => e.ProductId).IsRequired();

        builder.HasIndex(x => new { x.ProductId, x.WarehouseId }).IsUnique();
    }
}
