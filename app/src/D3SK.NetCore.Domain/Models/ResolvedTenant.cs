using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Domain.Models
{
    public class ResolvedTenant : ResolvedTenant<int?>
    {
        public ResolvedTenant(int? tenantId) : base(tenantId)
        {
        }
    }

    public class ResolvedTenant<TKey>
    {
        public TKey TenantId { get; }

        public ResolvedTenant(TKey tenantId)
        {
            TenantId = tenantId;
        }
    }
}
