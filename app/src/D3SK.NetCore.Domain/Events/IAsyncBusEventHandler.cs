using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Events
{
    public interface IAsyncBusEventHandlerBase
    {
        Task HandleAsync(object domainEvent);
    }

    public interface IAsyncBusEventHandler<in TEvent> : IAsyncBusEventHandlerBase where TEvent : IEventBase
    {
        Task HandleAsync(TEvent domainEvent);
    }
}
