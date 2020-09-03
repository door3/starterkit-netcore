using D3SK.NetCore.Common.Extensions;
using System.Text.Json.Serialization;

namespace D3SK.NetCore.Domain.Events.EntityEvents
{
    public abstract class EntityDomainEventBase : DomainEventBase, IEntityDomainEvent
    {
        public object Entity { get; }

        public string EntityName => Entity.GetType().Name;

        public string EntityAssemblyName => Entity.GetType().FullName;

        public object EntityId { get; }

        protected EntityDomainEventBase(object entity, object entityId)
        {
            Entity = entity;
            EntityId = entityId;
        }
    }
}
