using System.ComponentModel.DataAnnotations;

namespace Nutrifica.Api.Contracts.PhoneCalls;

public record PhoneCallCreateDto
{
    [Required(AllowEmptyStrings = false)] public Guid ClientId { get; set; }
    [Required(AllowEmptyStrings = false)] public string Comment { get; set; } = string.Empty;
    // возможно нужны поля статуса, не взяли трубку, занято
}