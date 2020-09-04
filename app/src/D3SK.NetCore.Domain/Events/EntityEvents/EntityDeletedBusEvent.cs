using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Domain.Events.EntityEvents
{
    public class EntityDeletedBusEvent : EntityBusEventBase, IEntityDeletedBusEvent
    {
        public EntityDeletedBusEvent(object entity, object entityId) : base(entity, entityId)
        {
        }
    }
}
