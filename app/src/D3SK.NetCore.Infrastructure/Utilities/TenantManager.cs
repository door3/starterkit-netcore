using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain.Models;

namespace D3SK.NetCore.Infrastructure.Utilities
{
    public class TenantManager : ITenantManager
    {
        public int? TenantId { get; private set; }

        public TenantManager(ResolvedTenant resolvedTenant)
        {
            TenantId = resolvedTenant.TenantId;
        }

        public virtual void SetTenantId(int? tenantId)
        {
            TenantId = tenantId;
        }
    }
}
