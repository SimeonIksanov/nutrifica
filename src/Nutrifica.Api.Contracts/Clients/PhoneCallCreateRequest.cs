using System.ComponentModel.DataAnnotations;

namespace Nutrifica.Api.Contracts.Clients;

public record PhoneCallCreateRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Comment { get; set; } = string.Empty;
    // возможно нужны поля статуса, не взяли трубку, занято
}