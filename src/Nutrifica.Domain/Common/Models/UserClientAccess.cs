using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Domain.Common.Models;

public class UserClientAccess : IAuditableEntity
{
    public UserId UserId { get; set; } = null!;
    public ClientId ClientId { get; set; } = null!;
    public UserClientAccessLevel AccessLevel { get; set; }
    public DateTime CreatedOn { get; set; }
    public UserId LastModifiedBy { get; set; } = null!;
    public DateTime LastModifiedOn { get; set; }
    public UserId CreatedBy { get; set; } = null!;
}