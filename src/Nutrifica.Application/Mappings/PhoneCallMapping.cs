using Nutrifica.Api.Contracts.PhoneCalls;
using Nutrifica.Application.Models.Clients;
using Nutrifica.Application.Models.PhoneCalls;

namespace Nutrifica.Application.Mappings;

public static class PhoneCallMapping
{
    public static PhoneCallDto ToPhoneCallDto(this PhoneCallModel phoneCall)
    {
        return new PhoneCallDto
        {
            Comment = phoneCall.Comment,
            CreatedOn = phoneCall.CreatedOn,
            CreatedBy = phoneCall.CreatedBy.ToUserShortDto(),
            Id = phoneCall.Id.Value,
        };
    }
}