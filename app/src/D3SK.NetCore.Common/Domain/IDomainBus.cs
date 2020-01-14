using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Events;

namespace D3SK.NetCore.Common.Domain
{
    public interface IDomainBus
    {
        IHandleDomainEventsStrategy<IDomainBusEvent> Events { get; }
    }
}
