using System.Threading.Tasks;
using D3SK.NetCore.Domain.Events;
using D3SK.NetCore.Domain.Events.EntityEvents;

namespace ExampleBookstore.Services.BookService.Infrastructure.Events
{
    public class EntityUpdatedEventHandler : AsyncDomainEventHandlerBase<EntityUpdatedBusEvent>
    {
        public override Task HandleAsync(EntityUpdatedBusEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}
