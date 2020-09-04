namespace D3SK.NetCore.Domain.Events.EntityEvents
{
    public abstract class EntityBusEventBase : BusEventBase, IEntityBusEvent
    {
        public object Entity { get; }

        public string EntityName => Entity.GetType().Name;

        public string EntityAssemblyName => Entity.GetType().FullName;

        public object EntityId { get; }

        protected EntityBusEventBase(object entity, object entityId)
        {
            Entity = entity;
            EntityId = entityId;
        }
    }
}
