using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Events
{
    public abstract class AsyncBusEventHandlerBase<TEvent> : IAsyncBusEventHandler<TEvent> where TEvent : IEventBase
    {
        public abstract Task HandleAsync(TEvent domainEvent);

        public Task HandleAsync(object domainEvent) => HandleAsync((TEvent)domainEvent);
    }
}
