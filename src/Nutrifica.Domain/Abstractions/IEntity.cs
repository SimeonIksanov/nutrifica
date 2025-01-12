namespace Nutrifica.Domain.Abstractions;

public interface IEntity
{
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}