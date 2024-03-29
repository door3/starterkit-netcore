﻿using System;

namespace D3SK.NetCore.Domain.Events
{
    public abstract class DomainEventBase : IDomainEvent
    {
        public Guid EventGuid { get; set; } = Guid.NewGuid();
    }
}
