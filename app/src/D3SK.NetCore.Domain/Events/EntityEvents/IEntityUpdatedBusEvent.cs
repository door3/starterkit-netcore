using System.Collections.Generic;
using D3SK.NetCore.Domain.Models;

namespace D3SK.NetCore.Domain.Events.EntityEvents
{
    public interface IEntityUpdatedBusEvent : IEntityBusEvent
    {
        IList<UpdatedEntityPropertyChange> PropertyChanges { get; }
    }
}
