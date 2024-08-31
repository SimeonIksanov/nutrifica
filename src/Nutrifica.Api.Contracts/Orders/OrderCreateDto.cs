namespace Nutrifica.Api.Contracts.Orders;

public record OrderCreateDto
{
    public Guid ClientId { get; set; }
}