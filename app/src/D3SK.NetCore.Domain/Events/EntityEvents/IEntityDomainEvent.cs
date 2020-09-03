namespace D3SK.NetCore.Domain.Events.EntityEvents
{
    public interface IEntityDomainEvent : IDomainBusEvent
    {
        object Entity { get; }

        string EntityName { get; }

        string EntityAssemblyName { get; }

        object EntityId { get; }
    }
}
