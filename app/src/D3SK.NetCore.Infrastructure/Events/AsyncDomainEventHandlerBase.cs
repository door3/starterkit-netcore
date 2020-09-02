using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Domain.Events;

namespace D3SK.NetCore.Infrastructure.Events
{
    public abstract class AsyncDomainEventHandlerBase<TEvent> : IAsyncDomainEventHandler<TEvent>
        where TEvent : IDomainEventBase
    {
        public abstract Task HandleAsync(TEvent domainEvent);
    }
}
