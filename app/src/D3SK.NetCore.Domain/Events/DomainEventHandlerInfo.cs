using D3SK.NetCore.Common.Extensions;
using System;

namespace D3SK.NetCore.Domain.Events
{
    public class DomainEventHandlerInfo
    {
        public Type ObjectType { get; }

        public Type HandlerType { get; }

        public HandleDomainEventOptions Options { get; }

        public DomainEventHandlerInfo(Type objectType, Type handlerType, HandleDomainEventOptions options)
        {
            ObjectType = objectType.NotNull(nameof(objectType));
            HandlerType = handlerType;
            Options = options;
        }
    }
}
