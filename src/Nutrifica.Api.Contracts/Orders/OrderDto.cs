using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Api.Contracts.Orders;

public record OrderDto
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public UserShortDto CreatedBy { get; set; } = null!;
    public ClientShortDto Client { get; set; } = null!;
    public OrderStatus Status { get; set; }
    public MoneyDto TotalSum { get; set; } = null!;
    public ICollection<OrderItemDto> OrderItems { get; set; } = null!;
    public ICollection<UserShortDto> Managers { get; set; } = null!;
}