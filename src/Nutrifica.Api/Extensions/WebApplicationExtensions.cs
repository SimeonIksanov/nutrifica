using Microsoft.AspNetCore.Identity;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Domain.UserAggregate;
using Nutrifica.Infrastructure.Persistence;

namespace Nutrifica.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void SeedDevelopmentData(this WebApplication app)
    {
        var scope = app
            .Services.CreateScope()
            .ServiceProvider;
        using var context = scope
            .GetRequiredService<AppDbContext>();
        var passHasher = scope.GetRequiredService<IPasswordHasherService>();

        // context.Users.Add(CreateMainUser(passHasher));
        context.SaveChanges();
    }

    private static User CreateMainUser(IPasswordHasherService ph)
    {
        var user = User.Create(username: "admin",
            firstName: "f",
            middleName: "m",
            lastname: "l",
            phoneNumber: null,
            email: "email",
            supervisorId: null);
        
        var ps = ph.HashPassword("admin");
        
        user.Account.PasswordHash = ps.hashed;
        user.Account.Salt = ps.salt;
        
        return user;
    }
}