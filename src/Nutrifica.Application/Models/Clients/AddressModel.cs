namespace Nutrifica.Application.Models.Clients;

public record AddressModel
{
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
}