using System.Threading.Tasks;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;
using D3SK.NetCore.Domain.Events.EntityEvents;
using ExampleBookstore.Services.BookService.Domain;

namespace ExampleBookstore.Services.BookService.Infrastructure.Events
{
    public class EntityUpdatedBusEventHandler : AsyncBusEventHandlerBase<EntityUpdatedBusEvent>
    {
        public override Task HandleAsync(EntityUpdatedBusEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}
