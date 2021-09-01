using System;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;
using Microsoft.Extensions.Logging;

namespace D3SK.NetCore.Infrastructure.Domain
{
    public class MonolithicDomainBus : IDomainBus
    {
        public IHandleBusEventStrategy<IBusEvent> EventStrategy { get; }
        private readonly ILogger<MonolithicDomainBus> _logger;

        public MonolithicDomainBus(IHandleBusEventStrategy<IBusEvent> eventStrategy,
            ILogger<MonolithicDomainBus> logger)
        {
            EventStrategy = eventStrategy.NotNull(nameof(eventStrategy));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> PublishEventAsync<TEvent>(TEvent busEvent, IDomainInstance domainInstance)
            where TEvent : IBusEvent
        {
            _logger.LogTrace("Publishing event {@busEvent}", busEvent);
            await EventStrategy.HandleEventAsync(busEvent, domainInstance.ServiceProvider);
            _logger.LogTrace("Event completed {@busEvent}", busEvent);
            return busEvent.EventGuid;
        }
    }
}
