namespace D3SK.NetCore.Domain.Events.EntityEvents
{
    public class EntityAddedBusEvent : EntityBusEventBase, IEntityAddedBusEvent
    {
        public EntityAddedBusEvent(object entity) : base(entity, null)
        {
        }
    }
}
