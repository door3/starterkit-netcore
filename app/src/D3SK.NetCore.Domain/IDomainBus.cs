﻿using System;
using System.Threading.Tasks;
using D3SK.NetCore.Domain.Events;

namespace D3SK.NetCore.Domain
{
    public interface IDomainBus
    {
        IHandleBusEventStrategy<IBusEvent> EventStrategy { get; }

        Task<Guid> PublishEventAsync<TEvent>(TEvent busEvent, IDomainInstance domainInstance) where TEvent : IBusEvent;
    }
}
