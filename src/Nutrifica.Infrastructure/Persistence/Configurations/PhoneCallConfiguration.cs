using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.PhoneCallAggregate;
using Nutrifica.Domain.Aggregates.PhoneCallAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Infrastructure.Persistence.Configurations;

public class PhoneCallConfiguration : IEntityTypeConfiguration<PhoneCall>
{
    public void Configure(EntityTypeBuilder<PhoneCall> builder)
    {
        builder.ToTable("PhoneCalls");
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                x => PhoneCallId.Create(x));
        builder
            .Property(phoneCall => phoneCall.ClientId)
            .HasConversion(
                x => x.Value,
                x => ClientId.Create(x));

        builder
            .Property(phoneCall => phoneCall.CreatedBy)
            .HasConversion(
                x => x.Value,
                x => UserId.Create(x));

        builder
            .Property(phoneCall => phoneCall.LastModifiedBy)
            .HasConversion(
                x => x.Value,
                x => UserId.Create(x));

        builder
            .HasOne<Client>()
            .WithMany()
            .HasForeignKey(product => product.ClientId).IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

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