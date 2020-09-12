using System;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;

namespace D3SK.NetCore.Infrastructure.Domain
{
    public class MonolithicDomainBus : IDomainBus
    {
        public IHandleBusEventStrategy<IBusEvent> EventStrategy { get; }

        public MonolithicDomainBus(IHandleBusEventStrategy<IBusEvent> eventStrategy)
        {
            EventStrategy = eventStrategy.NotNull(nameof(eventStrategy));
        }

        public async Task<Guid> PublishEventAsync<TEvent>(TEvent busEvent, IDomainInstance domainInstance)
            where TEvent : IBusEvent
        {
            await EventStrategy.HandleEventAsync(busEvent, domainInstance.ServiceProvider);
            return busEvent.EventGuid;
        }
    }
}
