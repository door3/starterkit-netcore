using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain.Models;
using Microsoft.Extensions.Options;

namespace D3SK.NetCore.Infrastructure.Utilities
{
    public class ClaimsTenantResolver : CurrentUserClaimsTenantResolver<ITenantUserClaims>
    {
        public ClaimsTenantResolver(IOptions<MultitenancyOptions> options,
            ICurrentUserManager<ITenantUserClaims> currentUserManager) : base(options, currentUserManager)
        {
        }
    }
}
