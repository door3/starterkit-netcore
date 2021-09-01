using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace D3SK.NetCore.Infrastructure.Events
{
    public class HandleBusEventStrategy<TEventBase> : IHandleBusEventStrategy<TEventBase>
        where TEventBase : IEventBase
    {
        public IList<DomainEventHandlerInfo> EventHandlers { get; } = new List<DomainEventHandlerInfo>();

        private readonly ILogger<HandleBusEventStrategy<TEventBase>> _logger;

        public HandleBusEventStrategy(ILogger<HandleBusEventStrategy<TEventBase>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void AddAsyncHandler<THandler, TEvent>(HandleDomainEventOptions options = null)
            where THandler : class, IAsyncBusEventHandler<TEvent> where TEvent : TEventBase
        {
            EventHandlers.Add(new DomainEventHandlerInfo(typeof(TEvent), typeof(THandler), options));
        }

        public async Task HandleEventAsync<TEvent>(TEvent domainEvent, IServiceProvider serviceProvider)
            where TEvent : TEventBase
        {
            _logger.LogTrace("Started handling event {@domainEvent}", domainEvent);
            var eventType = domainEvent.GetType();
            foreach (var type in eventType.GetBaseTypes(true))
            {
                var handlers = EventHandlers.Where(x => x.ObjectType == type);
                foreach (var handler in handlers)
                {
                    if (!handler.HandlerType?.ImplementsInterface<IAsyncBusEventHandlerBase>() ?? true)
                    {
                        return;
                    }

                    var eventHandler = (IAsyncBusEventHandlerBase) ActivatorUtilities.CreateInstance(
                        serviceProvider,
                        handler.HandlerType);
                    await eventHandler.HandleAsync(domainEvent);
                }
            }
            _logger.LogTrace("Finished handling event {@domainEvent}", domainEvent);
        }
    }
}
