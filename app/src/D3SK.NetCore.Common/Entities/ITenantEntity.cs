using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Entities
{
    public interface ITenantEntity : ITenantEntity<int>
    {
    }

    public interface ITenantEntity<TTenantKey> : IEntityBase
    {
        TTenantKey TenantId { get; }

        void SetTenantId(TTenantKey tenantId);
    }
}
