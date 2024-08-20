using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Api.Contracts.Orders;

public record OrderDto
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public UserShortDto CreatedBy { get; set; } = null!;
    public Guid ClientId { get; set; }
    public OrderStatus Type { get; set; }
    public Status Status { get; set; }
    public MoneyDto TotalSum { get; set; } = null!;
    public ICollection<OrderItemDto> OrderItems { get; set; } = null!;
    public ICollection<Guid> Managers { get; set; } = null!;
}