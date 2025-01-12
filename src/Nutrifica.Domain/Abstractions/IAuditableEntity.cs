using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Domain.Abstractions;

public interface IAuditableEntity
{
    UserId? CreatedBy { get; set; }
    DateTime CreatedOn { get; set; }
    UserId? LastModifiedBy { get; set; }
    DateTime LastModifiedOn { get; set; }
}