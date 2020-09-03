using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Domain.Events.EntityEvents
{
    public class EntityAddedDomainEvent : EntityDomainEventBase, IEntityAddedDomainEvent
    {
        public EntityAddedDomainEvent(object entity) : base(entity, null)
        {
        }
    }
}
