using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.Entities;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        ConfigureProperties(builder);
        ConfigureAccountTable(builder);
    }

    private static void ConfigureProperties(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                x => UserId.Create(x));
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
        builder.Property(x => x.Email)
            .HasConversion<string>(
                x => x.Value,
                x => Email.Create(x));
        builder.Property(x => x.PhoneNumber)
            .HasConversion<string>(
                x => x.Value,
                x => PhoneNumber.Create(x));
        builder.Property(x => x.SupervisorId)
            .HasConversion(
                x => x!.Value,
                x => UserId.Create(x));
        builder.Ignore(x => x.FullName);

        builder.Property(x => x.CreatedBy)
            .HasConversion(x => x.Value, x => UserId.Create(x));
        builder.Property(x => x.LastModifiedBy)
            .HasConversion(x => x.Value, x => UserId.Create(x));

        // builder.Property(x => x.Role).HasConversion<string>();
    }

    private static void ConfigureAccountTable(EntityTypeBuilder<User> builder)
    {
        // builder
        //     .HasOne<UserAccount>(user => user.Account)
        //     .WithOne(userAccount => userAccount.User)
        //     .HasForeignKey<UserAccount>("userId")
        //     .IsRequired();
        builder.OwnsOne<UserAccount>(u => u.Account, uab =>
        {
            uab.ToTable("UserAccount");
            uab.WithOwner().HasForeignKey("UserId");
            uab.HasKey("Id", "UserId");

            uab.OwnsMany(x => x.RefreshTokens, rtb =>
            {
                rtb.ToTable("RefreshTokens");
                rtb.WithOwner().HasForeignKey("AccountId", "UserId");
                rtb.HasKey(nameof(RefreshToken.Id), "UserId", "AccountId");

                rtb.Ignore(x => x.IsActive);
                rtb.Ignore(x => x.IsRevoked);
                rtb.Ignore(x => x.IsExpired);
            });
        });
    }
}