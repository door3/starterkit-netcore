using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using D3SK.NetCore.Domain.Events;

namespace D3SK.NetCore.Domain.Entities
{
    public interface IDomainEntity
    {
        [JsonIgnore]
        IList<IDomainEvent> DomainEvents { get; }

        void AddDomainEvent(IDomainEvent domainEvent);

        void ClearDomainEvents();
    }
}
