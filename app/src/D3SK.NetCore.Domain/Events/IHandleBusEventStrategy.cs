using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Events
{
    public interface IHandleBusEventStrategy<in TEventBase> where TEventBase : IEventBase
    {
        IList<DomainEventHandlerInfo> EventHandlers { get; }

        void AddAsyncHandler<THandler, TEvent>(HandleDomainEventOptions options = null)
            where THandler : class, IAsyncBusEventHandler<TEvent>
            where TEvent : TEventBase;

        Task HandleEventAsync<TEvent>(TEvent domainEvent, IServiceProvider serviceProvider) where TEvent : TEventBase;
    }
}
