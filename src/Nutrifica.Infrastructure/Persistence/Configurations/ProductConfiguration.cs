using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Nutrifica.Domain.Aggregates.ProductAggregate;
using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        ConfigureProperties(builder);
    }

    private void ConfigureProperties(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => ProductId.Create(x));

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.CreatedBy)
            .HasConversion(x => x.Value, x => UserId.Create(x));

        builder
            .Property(x => x.LastModifiedBy)
            .HasConversion(x => x.Value, x => UserId.Create(x));

        builder.OwnsOne(x => x.Price, mb =>
        {
            mb.OwnsOne(x => x.Currency);
        });

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(product => product.CreatedBy).IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(product => product.LastModifiedBy).IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}