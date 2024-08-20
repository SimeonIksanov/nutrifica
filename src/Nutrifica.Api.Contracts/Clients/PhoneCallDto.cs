using Nutrifica.Api.Contracts.Users.Responses;

namespace Nutrifica.Api.Contracts.Clients;

public class PhoneCallDto
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public UserShortDto CreatedBy { get; set; }
    public string Comment { get; set; } = string.Empty;
}