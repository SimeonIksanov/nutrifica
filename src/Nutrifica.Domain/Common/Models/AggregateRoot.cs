// namespace Nutrifica.Domain.Common.Models;

// public abstract class AggregateRoot<TId> : Entity<TId> where TId : notnull
// {
//     protected AggregateRoot()
//     {
//         
//     }
//     protected AggregateRoot(TId value)
//     {
//         Value = value;
//     }
//     public TId Value { get; protected set; }
// }


// public abstract class AggregateRoot<TId, TIdType> : Entity<TId> where TId : AggregateRootId<TIdType>
// {
//     protected AggregateRoot(TId id) : base(id)
//     {
//     }
//
//     protected AggregateRoot()
//     {
//     }
// }

// public abstract class AggregateRootId<TId> : ValueObject
// {
//     public TId Value { get; protected set; }
// }
