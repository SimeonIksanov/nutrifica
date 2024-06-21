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

        builder
            .HasOne<UserAccount>(user => user.Account)
            .WithOne(userAccount => userAccount.User)
            .HasForeignKey<UserAccount>("userId")
            .IsRequired();
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
        // builder.Property(x => x.Role).HasConversion<string>();
    }
}