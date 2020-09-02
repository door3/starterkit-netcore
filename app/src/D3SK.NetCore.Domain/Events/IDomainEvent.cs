using System;

namespace D3SK.NetCore.Domain.Events
{
    public interface IDomainEvent : IDomainEventBase
    {
        Guid EventGuid { get; set; }
    }
}
