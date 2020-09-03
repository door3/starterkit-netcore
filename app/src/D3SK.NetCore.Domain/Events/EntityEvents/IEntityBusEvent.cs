namespace D3SK.NetCore.Domain.Events.EntityEvents
{
    public interface IEntityBusEvent : IBusEvent
    {
        object Entity { get; }

        string EntityName { get; }

        string EntityAssemblyName { get; }

        object EntityId { get; }
    }
}
