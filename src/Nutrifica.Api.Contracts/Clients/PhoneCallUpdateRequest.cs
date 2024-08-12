using System.ComponentModel.DataAnnotations;

namespace Nutrifica.Api.Contracts.Clients;

public record PhoneCallUpdateRequest
{
    [Required]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Comment { get; set; } = string.Empty;
}