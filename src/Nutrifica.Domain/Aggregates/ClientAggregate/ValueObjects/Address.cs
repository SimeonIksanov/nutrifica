using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;

public class Address : ValueObject
{
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ZipCode;
        yield return Country;
        yield return Region;
        yield return City;
        yield return Street;
        yield return Comment;
    }
}
