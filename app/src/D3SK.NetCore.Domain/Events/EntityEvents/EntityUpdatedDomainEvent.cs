using System.Collections.Generic;
using D3SK.NetCore.Domain.Models;

namespace D3SK.NetCore.Domain.Events.EntityEvents
{
    public class EntityUpdatedDomainEvent : EntityDomainEventBase, IEntityUpdatedDomainEvent
    {
        public IList<UpdatedEntityPropertyChange> PropertyChanges { get; }

        public EntityUpdatedDomainEvent(object entity, object entityId, IList<UpdatedEntityPropertyChange> changes) :
            base(entity, entityId)
        {
            PropertyChanges = changes;
        }
    }
}