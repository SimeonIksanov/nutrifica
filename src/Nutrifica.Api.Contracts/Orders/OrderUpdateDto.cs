using Nutrifica.Shared.Enums;

namespace Nutrifica.Api.Contracts.Orders;

public record OrderUpdateDto
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public ICollection<Guid> ManagerIds { get; set; } = null!;
}