﻿using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Domain.Events
{
    public interface IDomainEventBase : IEventBase
    {
        Guid EventGuid { get; set; }
    }
}
