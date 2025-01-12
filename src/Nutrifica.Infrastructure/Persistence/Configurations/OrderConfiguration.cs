using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.OrderAggregate;
using Nutrifica.Domain.Aggregates.OrderAggregate.Entities;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        ConfigureProperties(builder);
        ConfigureOrderItemsTable(builder);
    }

    private void ConfigureOrderItemsTable(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsMany(x => x.OrderItems, oib =>
        {
            oib.ToTable("OrderItems");
            oib.WithOwner().HasForeignKey("OrderId");
            oib.HasKey(nameof(OrderItem.Id), "OrderId");
            oib.Property(x => x.ProductId)
                .HasConversion(
                    x => x.Value,
                    x => ProductId.Create(x));
            oib.OwnsOne(x => x.UnitPrice, mb =>
            {
                mb.OwnsOne(x => x.Currency);
            });
        });
        builder
            .Navigation(x => x.OrderItems)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }


    private void ConfigureProperties(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                x => OrderId.Create(x));

        builder
            .Property(x => x.ClientId)
            .HasConversion(
                x => x.Value,
                x => ClientId.Create(x));

        builder
            .Property(x => x.CreatedBy)
            .HasConversion(x => x.Value, x => UserId.Create(x));

        builder
            .Property(x => x.LastModifiedBy)
            .HasConversion(x => x.Value, x => UserId.Create(x));

        builder
            .OwnsOne(x => x.TotalSum, mb =>
            {
                mb.OwnsOne(x => x.Currency);
            });

        builder
            .Property(order => order.State)
            .HasConversion<string>();

        builder
            .Property(order => order.Status)
            .HasConversion<string>();

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(order => order.CreatedBy)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(order => order.LastModifiedBy)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<Client>()
            .WithMany()
            .HasForeignKey(order => order.ClientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}