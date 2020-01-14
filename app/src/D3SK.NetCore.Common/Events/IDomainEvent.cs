using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Events
{
    public interface IDomainEvent : IDomainEvent<int>
    {
    }

    public interface IDomainEvent<TTenantKey>
    {
        TTenantKey TenantId { get; }

        void SetTenantId(TTenantKey tenantId);

        object GetEventData();
    }

    public interface IEntityDomainEvent : IEntityDomainEvent<int, int>
    {
    }

    public interface IEntityDomainEvent<out TKey, TTenantKey> : IDomainEvent<TTenantKey>
    {
        TKey EntityId { get; }
    }

    public interface IDomainBusEvent : IDomainEvent
    {
    }
}
