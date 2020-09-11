using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Events
{
    public interface IAsyncDomainEventHandlerBase
    {
        Task HandleAsync(object domainEvent, IDomainInstance domainInstance);
    }

    public interface IAsyncDomainEventHandler<in TEvent, TDomain> : IAsyncDomainEventHandlerBase
        where TEvent : IEventBase
        where TDomain : IDomain
    {
        Task HandleAsync(TEvent domainEvent, IDomainInstance<TDomain> domainInstance);
    }
}