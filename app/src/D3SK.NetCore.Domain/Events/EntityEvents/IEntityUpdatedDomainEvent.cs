using System.Collections.Generic;
using D3SK.NetCore.Domain.Models;

namespace D3SK.NetCore.Domain.Events.EntityEvents
{
    public interface IEntityUpdatedDomainEvent : IEntityDomainEvent
    {
        IList<UpdatedEntityPropertyChange> PropertyChanges { get; }
    }
}
