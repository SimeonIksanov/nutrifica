using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Nutrifica.Domain.Aggregates.OrderAggregate;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Infrastructure.Persistence.Configurations;

public class UserOrderAccessConfiguration : IEntityTypeConfiguration<UserOrderAccess>
{
    public void Configure(EntityTypeBuilder<UserOrderAccess> builder)
    {
        builder.ToTable("UserOrderAccess");

        builder
            .HasKey(x => new { x.UserId, x.OrderId });

        builder
            .Property(x => x.UserId)
            .HasConversion(
                userId => userId.Value,
                guid => UserId.Create(guid));

        builder
            .Property(x => x.OrderId)
            .HasConversion(
                userId => userId.Value,
                guid => OrderId.Create(guid));

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
            .HasOne<Order>()
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.LastModifiedBy)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}