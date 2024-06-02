using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Nutrifica.Application.Models.Authentication;

public class RefreshToken
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    public string CreatedByIp { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string? ReasonRevoked { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? RevokedByIp { get; set; }

    public DateTimeOffset Expires { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }

    public bool IsActive => !IsRevoked && !IsExpired;
    public bool IsExpired => DateTimeOffset.UtcNow > Expires;
    public bool IsRevoked => RevokedAt != null;
}