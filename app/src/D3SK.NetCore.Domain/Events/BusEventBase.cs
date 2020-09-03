using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Domain.Events
{
    public abstract class BusEventBase : IBusEvent
    {
        public Guid EventGuid { get; set; } = Guid.NewGuid();
    }
}
