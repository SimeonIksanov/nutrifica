using System.ComponentModel.DataAnnotations.Schema;

namespace Nutrifica.Domain.UserAggregate.Entities;

public class UserAccount
{
    public User User { get; set; } = null!;
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;
}