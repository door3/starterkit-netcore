﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace D3SK.NetCore.Infrastructure.Events
{
    public class HandleDomainEventStrategy<TEventBase, TDomain> : IHandleDomainEventStrategy<TEventBase, TDomain>
        where TEventBase : IEventBase
        where TDomain : IDomain
    {
        public IList<DomainEventHandlerInfo> EventHandlers { get; } = new List<DomainEventHandlerInfo>();

        public void AddAsyncHandler<THandler, TEvent>(HandleDomainEventOptions options = null)
            where THandler : class, IAsyncDomainEventHandler<TEvent, TDomain> where TEvent : TEventBase
        {
            EventHandlers.Add(new DomainEventHandlerInfo(typeof(TEvent), typeof(THandler), options));
        }

        public async Task HandleEventAsync<TEvent>(TEvent domainEvent, IServiceProvider serviceProvider, IDomainInstance<TDomain> domainInstance)
            where TEvent : TEventBase
        {
            var eventType = domainEvent.GetType();
            foreach (var type in eventType.GetBaseTypes(true))
            {
                var handlers = EventHandlers.Where(x => x.ObjectType == type);
                foreach (var handler in handlers)
                {
                    if (!handler.HandlerType?.ImplementsInterface<IAsyncDomainEventHandlerBase>() ?? true)
                    {
                        continue;
                    }

                    var eventHandler = (IAsyncDomainEventHandlerBase)ActivatorUtilities.CreateInstance(
                        serviceProvider,
                        handler.HandlerType);
                    await eventHandler.HandleAsync(domainEvent, domainInstance);
                }
            }
        }
    }
}