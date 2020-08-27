using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Domain.Events;

namespace D3SK.NetCore.Infrastructure.Events
{
    // TODO: rework handle middleware events strategy
    public class HandleDomainMiddlewareStrategy<TMiddleware> : IHandleDomainMiddlewareStrategy<TMiddleware>
        where TMiddleware : IDomainMiddleware
    {
        public IList<DomainMiddlewareHandlerInfo> Handlers { get; }

        public void AddHandlerAsync<THandler>(HandleMiddlewareEventOptions options = null)
            where THandler : class, IAsyncDomainMiddlewareHandler<TMiddleware>
        {
            throw new NotImplementedException();
        }
    }
}