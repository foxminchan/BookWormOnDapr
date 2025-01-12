using BookWorm.Catalog.Domain;
using BookWorm.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookWorm.Catalog.Infrastructure.Data.EntityConfigurations;

internal sealed class CategoryConfiguartion : IEntityTypeConfiguration<Category>
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
