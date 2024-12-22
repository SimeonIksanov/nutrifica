using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Nutrifica.Application.Models.Users;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Infrastructure.Persistence.Configurations;

public class UserShortModelConfiguration : IEntityTypeConfiguration<UserShortModel>
{
    public void Configure(EntityTypeBuilder<UserShortModel> builder)
    {
        builder
            .Property(x => x.Id)
            .HasConversion(
                x => x.Value,
                x => UserId.Create(x));
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
    }
}