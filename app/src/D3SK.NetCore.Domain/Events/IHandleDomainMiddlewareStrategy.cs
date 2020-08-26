using System.Collections.Generic;

namespace D3SK.NetCore.Domain.Events
{
    public interface IHandleDomainMiddlewareStrategy<out TMiddleware> where TMiddleware : IDomainMiddleware
    {
        IList<DomainMiddlewareHandlerInfo> Handlers { get; }

        void AddHandlerAsync<THandler>(HandleMiddlewareEventOptions options = null)
            where THandler : class, IAsyncDomainMiddlewareHandler<TMiddleware>;
    }
}
