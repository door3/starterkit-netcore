using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Domain.Events.EntityEvents
{
    public class EntityDeletedDomainEvent : EntityDomainEventBase, IEntityDeletedDomainEvent
    {
        public EntityDeletedDomainEvent(object entity, object entityId) : base(entity, entityId)
        {
        }
    }
}
