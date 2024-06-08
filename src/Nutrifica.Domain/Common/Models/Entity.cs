using Nutrifica.Domain.Common.Interfaces;

namespace Nutrifica.Domain.Common.Models;

public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public TId Id { get; protected set; } = default(TId)!;

    protected Entity(TId id) => Id = id;
    protected Entity() { }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();
    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public bool Equals(Entity<TId>? other)
    {
        return other is not null && Id.Equals(other.Id);
    }
    public override bool Equals(object? other)
        => other is not null && other is Entity<TId> entity && Id.Equals(entity.Id);

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Entity<TId> left, Entity<TId> right) => Equals(left, right);
    public static bool operator !=(Entity<TId> left, Entity<TId> right) => !Equals(left, right);
}
