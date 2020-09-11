using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Events
{
    public interface IHandleDomainEventStrategy<in TEventBase, TDomain> 
        where TEventBase : IEventBase
        where TDomain : IDomain
    {
        IList<DomainEventHandlerInfo> EventHandlers { get; }

        void AddAsyncHandler<THandler, TEvent>(HandleDomainEventOptions options = null)
            where THandler : class, IAsyncDomainEventHandler<TEvent, TDomain>
            where TEvent : TEventBase;

        Task HandleEventAsync<TEvent>(TEvent domainEvent, IServiceProvider serviceProvider, IDomainInstance<TDomain> domainInstance)
            where TEvent : TEventBase;
    }
}
