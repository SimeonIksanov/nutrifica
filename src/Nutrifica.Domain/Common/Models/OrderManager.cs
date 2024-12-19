using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Domain.Common.Models;

public class OrderManager
{
    public OrderId OrderId { get; set; }
    public UserId UserId { get; set; }
}

public class ClientManager
{
    public ClientId ClientId { get; set; }
    public UserId UserId { get; set; }
}