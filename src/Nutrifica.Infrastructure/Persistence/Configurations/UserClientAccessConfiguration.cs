using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Infrastructure.Persistence.Configurations;

public class UserClientAccessConfiguration : IEntityTypeConfiguration<UserClientAccess>
{
    public void Configure(EntityTypeBuilder<UserClientAccess> builder)
    {
        builder.ToTable("UserClientAccess");

        builder
            .HasKey(x => new { x.UserId, x.ClientId });

        builder
            .Property(x => x.UserId)
            .HasConversion(
                userId => userId.Value,
                guid => UserId.Create(guid));

        builder
            .Property(x => x.ClientId)
            .HasConversion(
                userId => userId.Value,
                guid => ClientId.Create(guid));

        builder
            .Property(x => x.CreatedBy)
            .HasConversion(
                userId => userId.Value,
                guid => UserId.Create(guid));

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.CreatedBy)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Client>()
            .WithMany()
            .HasForeignKey(x => x.ClientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.AccessLevel)
            .HasConversion<string>();

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.LastModifiedBy)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}