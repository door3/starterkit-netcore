using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Domain.Events;
using D3SK.NetCore.Domain.Events.EntityEvents;
using D3SK.NetCore.Infrastructure.Events;

namespace BookService.Infrastructure.Events
{
    public class EntityUpdatedEventHandler : AsyncDomainEventHandlerBase<EntityUpdatedBusEvent>
    {
        public override Task HandleAsync(EntityUpdatedBusEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}
