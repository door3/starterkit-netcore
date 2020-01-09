using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Events
{
    public interface IHandleDomainEventsStrategy<in TEventType> where TEventType : IDomainEvent
    {
        IList<DomainEventHandlerInfo> EventHandlers { get; }

        void HandleEvent<TEvent, THandler>(HandleDomainEventOptions options = null)
            where TEvent : TEventType
            where THandler : class, IDomainEventHandler<TEvent>;

        void HandleAsyncEvent<TEvent, THandler>(HandleDomainEventOptions options = null)
            where TEvent : TEventType
            where THandler : class, IAsyncDomainEventHandler<TEvent>;
    }
}
