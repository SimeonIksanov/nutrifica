namespace Nutrifica.Domain.Aggregates.UserAggregate.Entities;

public class UserAccount
{
    public User User { get; set; } = null!;
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public List<RefreshToken> RefreshTokens { get; set; } = null!;
}
