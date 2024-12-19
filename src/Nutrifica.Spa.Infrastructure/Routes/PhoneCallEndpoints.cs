namespace Nutrifica.Spa.Infrastructure.Routes;

public static class PhoneCallEndpoints
{
    public static string GetPhoneCalls(Guid clientId) => $"api/phonecalls/client/{clientId.ToString()}";
    public static string CreatePhoneCall(Guid clientId) => $"api/phonecalls/client/{clientId.ToString()}";
    public static string UpdatePhoneCall(Guid clientId, Guid phoneCallId) => $"api/phonecalls/client/{clientId.ToString()}/{phoneCallId.ToString()}";
    public static string DeletePhoneCall(Guid clientId, Guid phoneCallId) => $"api/phonecalls/client/{clientId.ToString()}/{phoneCallId.ToString()}";
}