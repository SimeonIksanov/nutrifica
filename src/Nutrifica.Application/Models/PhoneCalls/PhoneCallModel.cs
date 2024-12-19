using Nutrifica.Application.Models.Clients;
using Nutrifica.Application.Models.Users;
using Nutrifica.Domain.Aggregates.PhoneCallAggregate.ValueObjects;

namespace Nutrifica.Application.Models.PhoneCalls;

public record PhoneCallModel
{
    public PhoneCallId Id { get; set; } = null!;
    public ClientShortModel Client { get; set; } = null!;
    public DateTime CreatedOn { get; set; }

    public UserShortModel CreatedBy { get; set; } = null!;

    // ICollection<ProductModel> products
    public string Comment { get; set; } = null!;
}
