using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Events
{
    public abstract class AsyncDomainEventHandlerBase<TEvent, TDomain> : IAsyncDomainEventHandler<TEvent, TDomain>
        where TEvent : IEventBase
        where TDomain : IDomain
    {
        public abstract Task HandleAsync(TEvent domainEvent, IDomainInstance<TDomain> domainInstance);

        public Task HandleAsync(object domainEvent, IDomainInstance domainInstance) =>
            HandleAsync((TEvent) domainEvent, (IDomainInstance<TDomain>) domainInstance);
    }
}
