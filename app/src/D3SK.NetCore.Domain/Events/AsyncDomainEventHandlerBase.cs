using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Events
{
    public abstract class AsyncDomainEventHandlerBase<TEvent> : IAsyncDomainEventHandler<TEvent>
        where TEvent : IDomainEventBase
    {
        public abstract Task HandleAsync(TEvent domainEvent);

        public Task HandleAsync(object domainEvent) => HandleAsync((TEvent)domainEvent);
    }
}
