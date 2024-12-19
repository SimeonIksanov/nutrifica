using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Infrastructure.Persistence.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                x => ClientId.Create(x));
        builder
            .Property(x => x.FirstName)
            .HasConversion<string>(
                x => x.Value,
                x => FirstName.Create(x));
        builder
            .Property(x => x.MiddleName)
            .HasConversion<string>(
                x => x.Value,
                x => MiddleName.Create(x));
        builder
            .Property(x => x.LastName)
            .HasConversion<string>(
                x => x.Value,
                x => LastName.Create(x));
        builder
            .Property(x => x.Comment)
            .HasConversion<string>(
                x => x.Value,
                x => Comment.Create(x));
        builder
            .Property(x => x.PhoneNumber)
            .HasConversion<string>(
                x => x.Value,
                x => PhoneNumber.Create(x));
        builder
            .Property(x => x.CreatedBy)
            .HasConversion(
                x => x.Value,
                x => UserId.Create(x));
        builder
            .Property(x => x.LastModifiedBy)
            .HasConversion(x => x.Value, x => UserId.Create(x));

        builder
            .OwnsOne(x => x.Address);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(client => client.CreatedBy)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(client => client.LastModifiedBy)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }

    // private void ConfigureOrdersTable(EntityTypeBuilder<Client> builder)
    // {
    //     builder.OwnsMany(x => x.OrderIds, oib =>
    //         {
    //             oib.ToTable("ClientOrderIds");
    //             oib.WithOwner().HasForeignKey("ClientId");
    //
    //             oib.HasKey(nameof(Order.Id));
    //             oib.Property(x => x.Value)
    //                 .HasColumnName("OrderId")
    //                 .ValueGeneratedNever();
    //         });
    //     builder.Metadata
    //         .FindNavigation(nameof(Client.OrderIds))!
    //         .SetPropertyAccessMode(PropertyAccessMode.Field);
    // }
}