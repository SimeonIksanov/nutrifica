using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Domain.Common.Models;

public class UserOrderAccess : IAuditableEntity
{
    public UserId UserId { get; set; } = null!;
    public OrderId OrderId { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public UserId LastModifiedBy { get; set; } = null!;
    public DateTime LastModifiedOn { get; set; }
    public UserId CreatedBy { get; set; } = null!;
}