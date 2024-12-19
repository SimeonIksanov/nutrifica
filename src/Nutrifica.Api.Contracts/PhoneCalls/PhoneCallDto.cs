using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Api.Contracts.Users.Responses;

namespace Nutrifica.Api.Contracts.PhoneCalls;

public class PhoneCallDto
{
    public Guid Id { get; set; }
    public ClientShortDto Client { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public UserShortDto CreatedBy { get; set; } = null!;
    public string Comment { get; set; } = string.Empty;
}