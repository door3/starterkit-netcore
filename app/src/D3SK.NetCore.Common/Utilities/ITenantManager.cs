using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Utilities
{
    public interface ITenantManager
    {
        int? TenantId { get; }

        void SetTenantId(int tenantId);
    }
}
