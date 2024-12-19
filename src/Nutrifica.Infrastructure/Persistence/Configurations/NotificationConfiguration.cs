using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Nutrifica.Domain.Aggregates.NotificationAggregate;
using Nutrifica.Domain.Aggregates.NotificationAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notification");

        builder.HasKey(notification => notification.Id);

        builder
            .Property(notification => notification.Id)
            .HasConversion(
                notificationId => notificationId.Value,
                guid => NotificationId.Create(guid));

        builder
            .Property(notification => notification.RecipientId)
            .HasConversion(
                userId => userId.Value,
                guid => UserId.Create(guid));

        builder
            .Property(notification => notification.CreatedBy)
            .HasConversion(
                userId => userId.Value,
                guid => UserId.Create(guid));

        builder
            .Property(notification => notification.LastModifiedBy)
            .HasConversion(
                userId => userId.Value,
                guid => UserId.Create(guid));

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(product => product.RecipientId).IsRequired()
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