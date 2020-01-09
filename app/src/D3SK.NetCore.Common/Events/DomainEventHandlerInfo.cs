using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Common.Events
{
    public class DomainEventHandlerInfo
    {
        public Type EventType { get; }

        public Type HandlerType { get; }

        public HandleDomainEventOptions Options { get; }

        public DomainEventHandlerInfo(Type eventType, Type handlerType, HandleDomainEventOptions options)
        {
            EventType = eventType.NotNull(nameof(eventType));
            HandlerType = handlerType.NotNull(nameof(handlerType));
            Options = options;
        }
    }
}
