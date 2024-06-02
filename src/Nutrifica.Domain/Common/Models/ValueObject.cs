namespace Nutrifica.Domain.Common.Models;

public abstract class ValueObject: IEquatable<ValueObject>
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public bool Equals(ValueObject? other)
    {
        if (other is null) return false;
        
        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    public override bool Equals(object? other)
    {
        if (other is null || other.GetType() != GetType())
            return false;
        
        return Equals(other);
    }

    public static bool operator ==(ValueObject left, ValueObject right) => left.Equals(right);

    public static bool operator !=(ValueObject left, ValueObject right) => !left.Equals(right);

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }
}