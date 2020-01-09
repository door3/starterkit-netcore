using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3SK.NetCore.Common.Events
{
    public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
    {
        void Handle(TEvent domainEvent);
    }

    public interface IAsyncDomainEventHandler<in TEvent>  where TEvent : IDomainEvent
    {
        Task HandleAsync(TEvent domainEvent);
    }
}
