using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Utilities
{
    public interface ITenantManager : ITenantManager<int?>
    {
    }

    public interface ITenantManager<TKey>
    {
        TKey TenantId { get; }

        void SetTenantId(TKey tenantId);
    }
}
