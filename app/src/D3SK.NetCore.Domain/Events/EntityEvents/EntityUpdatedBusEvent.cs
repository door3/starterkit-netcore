using System.Collections.Generic;
using D3SK.NetCore.Domain.Models;

namespace D3SK.NetCore.Domain.Events.EntityEvents
{
    public class EntityUpdatedBusEvent : EntityBusEventBase, IEntityUpdatedBusEvent
    {
        public IList<UpdatedEntityPropertyChange> PropertyChanges { get; }

        public EntityUpdatedBusEvent(object entity, object entityId, IList<UpdatedEntityPropertyChange> changes) :
            base(entity, entityId)
        {
            PropertyChanges = changes;
        }
    }
}