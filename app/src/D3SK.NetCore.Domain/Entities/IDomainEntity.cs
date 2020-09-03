using System.Collections.Generic;
using D3SK.NetCore.Domain.Events;

namespace D3SK.NetCore.Domain.Entities
{
    public interface IDomainEntity
    {
        [Newtonsoft.Json.JsonIgnore]
        IList<IDomainEventBase> DomainEvents { get; }

        void AddDomainEvent(IDomainEventBase domainEvent);

        void ClearAllDomainEvents();

        void ClearDomainEvents<TEvent>() where TEvent : IDomainEventBase;
    }
}
