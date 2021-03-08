using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Queries
{
    public interface IAllowNonOwnerPermissionQuery
    {
        int NonOwnerPermissionId { get; set; }

        bool CurrentUserHasNonOwnerPermission { get; set; }
    }
}
