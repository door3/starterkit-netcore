using System.Collections.Generic;
using D3SK.NetCore.Domain.Events;

namespace D3SK.NetCore.Domain.Entities
{
    public interface IDomainEntity
    {
        [Newtonsoft.Json.JsonIgnore]
        IList<IDomainEventBase> DomainEvents { get; }

        void AddDomainEvent<T>(T eventItem) where T : IDomainEventBase;

        void ClearAllDomainEvents();

        void ClearDomainEvents<TEvent>() where TEvent : IDomainEventBase;
    }
}
