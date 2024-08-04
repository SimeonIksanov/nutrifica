using Nutrifica.Api.Contracts.Users.Responses;

namespace Nutrifica.Api.Contracts.Clients;

public class PhoneCallResponse
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public UserFullNameResponse CreatedBy { get; set; }
    public string Comment { get; set; } = string.Empty;
}