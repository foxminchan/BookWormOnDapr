using BookWorm.Customer.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookWorm.Customer.Infrastructure.EntityConfigurations;

internal sealed class ConsumerConfiguration : IEntityTypeConfiguration<Consumer>
{
    public void Configure(EntityTypeBuilder<Consumer> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName).HasMaxLength(DataSchemaLength.Medium).IsRequired();

        builder.Property(e => e.LastName).HasMaxLength(DataSchemaLength.Medium).IsRequired();

        builder.Property(e => e.Email).HasMaxLength(DataSchemaLength.Large).IsRequired();

        builder.Property(e => e.PhoneNumber).HasMaxLength(DataSchemaLength.Medium).IsRequired();

        builder.Property(e => e.AccountId).HasMaxLength(DataSchemaLength.Medium);

        builder.Property(e => e.CreatedAt).HasDefaultValue(DateTime.UtcNow);

        builder.Property(e => e.LastModifiedAt).HasDefaultValue(DateTime.UtcNow);

        builder.Property(e => e.Version).IsConcurrencyToken();

        builder.HasIndex(e => e.IsDeleted);

        builder.OwnsOne(
            e => e.Address,
            a =>
            {
                a.Property(e => e.Street).HasMaxLength(DataSchemaLength.Large).IsRequired();
                a.Property(e => e.City).HasMaxLength(DataSchemaLength.Medium).IsRequired();
                a.Property(e => e.State).HasMaxLength(DataSchemaLength.Medium).IsRequired();
                a.Property(e => e.ZipCode).HasMaxLength(DataSchemaLength.Small).IsRequired();
                a.Property(e => e.Country).HasMaxLength(DataSchemaLength.Medium).IsRequired();
            }
        );
    }
}
