using Microsoft.AspNetCore.Identity;

using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Shared;
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

        var admin = CreateMainUser(passHasher); 
        context.Users.Add(admin);
        var users = CreateUsers(passHasher);

        users[1].SupervisorId = admin.Id;
        
        context.Users.AddRange(users);
        context.SaveChanges();
    }

    private static User CreateMainUser(IPasswordHasherService ph)
    {
        var user = User.Create(username: "admin",
            firstName: FirstName.Create("f"),
            middleName: MiddleName.Create("m"),
            lastname: LastName.Create("l"),
            phoneNumber: PhoneNumber.Create("123"),
            email: Email.Create("email"),
            supervisorId: null);

        var ps = ph.HashPassword("admin");

        user.Account.PasswordHash = ps.hashed;
        user.Account.Salt = ps.salt;

        return user;
    }

    private static List<User> CreateUsers(IPasswordHasherService ph)
    {
        var dictList = new List<Dictionary<string, string>>
        {
            new Dictionary<string, string>
            {
                { "firstName", "Leanne" },
                { "lastName", "Graham" },
                { "middleName", "" },
                { "username", "Bret" },
                { "password", "Bret" },
                { "email", "Sincere@april.biz" },
                { "phone", "1-770-736-8031 x56442" }
            },
            new Dictionary<string, string>
            {
                { "firstName", "Ervin" },
                { "lastName", "Howell" },
                { "middleName", "" },
                { "username", "Antonette" },
                { "password", "Antonette" },
                { "email", "Shanna@melissa.tv" },
                { "phone", "010-692-6593 x09125" }
            },
            new Dictionary<string, string>
            {
                { "firstName", "Clementine" },
                { "lastName", "Bauch" },
                { "middleName", "" },
                { "username", "Samantha" },
                { "password", "Samantha" },
                { "email", "Nathan@yesenia.net" },
                { "phone", "1-463-123-4447" }
            },
            new Dictionary<string, string>
            {
                { "firstName", "Patricia" },
                { "lastName", "Lebsack" },
                { "middleName", "" },
                { "username", "Karianne" },
                { "password", "Karianne" },
                { "email", "Julianne.OConner@kory.org" },
                { "phone", "493-170-9623 x156" }
            },
            new Dictionary<string, string>
            {
                { "firstName", "Chelsey" },
                { "lastName", "Dietrich" },
                { "middleName", "" },
                { "username", "Kamren" },
                { "password", "Kamren" },
                { "email", "Lucio_Hettinger@annie.ca" },
                { "phone", "(254)954-1289" }
            },
            new Dictionary<string, string>
            {
                { "firstName", "Dennis" },
                { "lastName", "Schulist" },
                { "middleName", "" },
                { "username", "Leopoldo_Corkery" },
                { "password", "Leopoldo_Corkery" },
                { "email", "Karley_Dach@jasper.info" },
                { "phone", "1-477-935-8478 x6430" }
            },
            new Dictionary<string, string>
            {
                { "firstName", "Kurtis" },
                { "lastName", "Weissnat" },
                { "middleName", "" },
                { "username", "Elwyn.Skiles" },
                { "password", "Elwyn.Skiles" },
                { "email", "Telly.Hoeger@billy.biz" },
                { "phone", "210.067.6132" }
            },
            new Dictionary<string, string>
            {
                { "firstName", "Nicholas" },
                { "lastName", "Runolfsdottir" },
                { "middleName", "" },
                { "username", "Maxime_Nienow" },
                { "password", "Maxime_Nienow" },
                { "email", "Sherwood@rosamond.me" },
                { "phone", "586.493.6943 x140" }
            },
            new Dictionary<string, string>
            {
                { "firstName", "Glenna" },
                { "lastName", "Reichert" },
                { "middleName", "" },
                { "username", "Delphine" },
                { "password", "Delphine" },
                { "email", "Chaim_McDermott@dana.io" },
                { "phone", "(775)976-6794 x41206" }
            },
            new Dictionary<string, string>
            {
                { "firstName", "Clementina" },
                { "lastName", "DuBuque" },
                { "middleName", "" },
                { "username", "Moriah.Stanton" },
                { "password", "Moriah.Stanton" },
                { "email", "Rey.Padberg@karina.biz" },
                { "phone", "024-648-3804" }
            }
        };
        var users = new List<User>(10);
        foreach (var dict in dictList)
        {
            var user = User.Create(dict["username"], FirstName.Create(dict["firstName"]),
                MiddleName.Create(dict["middleName"]), LastName.Create(dict["lastName"]),
                PhoneNumber.Create(dict["phone"]), Email.Create(dict["email"]), null);
            var ps = ph.HashPassword(dict["password"]);
            user.Account.PasswordHash = ps.hashed;
            user.Account.Salt = ps.salt;
            users.Add(user);
        }

        return users;
    }
}