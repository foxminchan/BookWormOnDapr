using BookWorm.Catalog.Domain;

namespace BookWorm.Catalog.Infrastructure.Data.EntityConfigurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(p => p.Name).HasMaxLength(DataSchemaLength.Large).IsRequired();

        builder
            .HasMany(x => x.BookCategories)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.BookCategories).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
