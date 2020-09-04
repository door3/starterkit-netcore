using System;

namespace D3SK.NetCore.Domain.Events
{
    public interface IDomainEventBase : IEventBase
    {
        Guid EventGuid { get; set; }
    }
}
