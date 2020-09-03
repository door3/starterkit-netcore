﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace D3SK.NetCore.Infrastructure.Domain
{
    public class MonolithicDomainBus : IDomainBus
    {
        public IHandleDomainEventStrategy<IDomainBusEvent> EventStrategy { get; }

        public MonolithicDomainBus(IHandleDomainEventStrategy<IDomainBusEvent> eventStrategy)
        {
            EventStrategy = eventStrategy.NotNull(nameof(eventStrategy));
        }

        public async Task<Guid> PublishEventAsync<TEvent>(TEvent busEvent, IDomainInstance domainInstance)
            where TEvent : IDomainBusEvent
        {
            await EventStrategy.HandleEventAsync(busEvent, domainInstance.ServiceProvider);
            return busEvent.EventGuid;
        }
    }
}
