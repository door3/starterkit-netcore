using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common;
using D3SK.NetCore.Common.Utilities;

namespace D3SK.NetCore.Domain.Stores
{
    public interface IDomainDbStoreManager
    {
        IClock Clock { get; }

        IDomainInstance DomainInstance { get; }

        ITenantManager TenantManager { get; }
    }
}
