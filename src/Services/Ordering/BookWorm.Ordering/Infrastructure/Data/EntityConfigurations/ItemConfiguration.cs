using BookWorm.Ordering.Domain;

namespace BookWorm.Ordering.Infrastructure.Data.EntityConfigurations;

internal sealed class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(p => p.Price).HasPrecision(18, 2).IsRequired();

        builder.Property(p => p.Quantity).IsRequired();
    }
}
