using D3SK.NetCore.Common.Extensions;
using System;

namespace D3SK.NetCore.Domain.Events
{
    public class DomainMiddlewareHandlerInfo
    {
        public Type ObjectType { get; }

        public Type HandlerType { get; }

        public HandleMiddlewareEventOptions Options { get; }

        public DomainMiddlewareHandlerInfo(Type objectType, Type handlerType, HandleMiddlewareEventOptions options)
        {
            ObjectType = objectType.NotNull(nameof(objectType));
            HandlerType = handlerType;
            Options = options;
        }
    }
}
