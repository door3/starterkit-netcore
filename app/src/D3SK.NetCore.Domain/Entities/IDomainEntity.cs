using System.Collections.Generic;
using D3SK.NetCore.Domain.Events;

namespace D3SK.NetCore.Domain.Entities
{
    public interface IDomainEntity
    {
        [Newtonsoft.Json.JsonIgnore]
        IList<IDomainEvent> DomainEvents { get; }

        void AddDomainEvent(IDomainEvent domainEvent);

        void ClearDomainEvents();
    }
}
