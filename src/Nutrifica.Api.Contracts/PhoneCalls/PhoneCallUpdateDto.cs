using System.ComponentModel.DataAnnotations;

namespace Nutrifica.Api.Contracts.PhoneCalls;

public record PhoneCallUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Comment { get; set; } = string.Empty;
}