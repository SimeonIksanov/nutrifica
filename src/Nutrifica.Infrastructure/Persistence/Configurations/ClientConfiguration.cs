using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.ClientAggregate.Entities;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.OrderAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Infrastructure.Persistence.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        ConfigureProperties(builder);
        ConfigureManagersTable(builder);
        ConfigureOrdersTable(builder);
        ConfigurePhoneCallsTable(builder);
    }

    private void ConfigurePhoneCallsTable(EntityTypeBuilder<Client> builder)
    {
        builder
            .OwnsMany(x => x.PhoneCalls, pcb =>
            {
                pcb.ToTable("ClientPhoneCalls");
                pcb.WithOwner().HasForeignKey("ClientId");

                pcb.HasKey(nameof(PhoneCall.Id), "ClientId");
                pcb.Property(x => x.Id).ValueGeneratedOnAdd();

                pcb
                    .Property(x => x.CreatedBy)
                    .HasConversion(
                        x => x.Value,
                        x => UserId.Create(x));
            });
        builder
            .Navigation(x => x.PhoneCalls)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureOrdersTable(EntityTypeBuilder<Client> builder)
    {
        builder
            .OwnsMany(x => x.OrderIds, oib =>
            {
                oib.ToTable("ClientOrderIds");
                oib.WithOwner().HasForeignKey("ClientId");

                oib.HasKey(nameof(Order.Id));
                oib.Property(x => x.Value)
                    .HasColumnName("OrderId")
                    .ValueGeneratedNever();
            });
        builder.Metadata
            .FindNavigation(nameof(Client.OrderIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureManagersTable(EntityTypeBuilder<Client> builder)
    {
        builder
            .OwnsMany(x => x.ManagerIds, mib =>
            {
                mib.ToTable("ClientManagerIds");
                mib.WithOwner().HasForeignKey("ClientId");

                mib.HasKey(nameof(User.Id));
                mib
                    .Property(x => x.Value)
                    .HasColumnName("ManagerId")
                    .ValueGeneratedNever();
            });

        builder
            .Navigation(x => x.ManagerIds)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureProperties(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                x => ClientId.Create(x));
        builder.Property(x => x.FirstName)
            .HasConversion<string>(
                x => x.Value,
                x => FirstName.Create(x));
        builder.Property(x => x.MiddleName)
            .HasConversion<string>(
                x => x.Value,
                x => MiddleName.Create(x));
        builder.Property(x => x.LastName)
            .HasConversion<string>(
                x => x.Value,
                x => LastName.Create(x));
        builder.Property(x => x.Comment)
            .HasConversion<string>(
                x => x.Value,
                x => Comment.Create(x));
        builder.Property(x => x.PhoneNumber)
            .HasConversion<string>(
                x => x.Value,
                x => PhoneNumber.Create(x));
        builder.Property(x => x.CreatedBy)
            .HasConversion(
                x => x.Value,
                x => UserId.Create(x));

        builder
            .OwnsOne(x => x.Address);
    }
}