using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Queries
{
    public interface IAllowNonOwnerPermissionCommand : IAllowNonOwnerPermissionQuery
    {
        int OwnerPermissionId { get; set; }
    }
}
