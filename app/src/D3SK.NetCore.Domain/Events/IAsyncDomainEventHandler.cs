using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Events
{
    public interface IAsyncDomainEventHandlerBase
    {
        Task HandleAsync(object domainEvent);
    }

    public interface IAsyncDomainEventHandler<in TEvent> : IAsyncDomainEventHandlerBase where TEvent : IDomainEventBase
    {
        Task HandleAsync(TEvent domainEvent);
    }
}