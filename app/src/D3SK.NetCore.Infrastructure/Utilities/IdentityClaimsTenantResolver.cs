using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain.Models;
using Microsoft.Extensions.Options;

namespace D3SK.NetCore.Infrastructure.Utilities
{
    public class IdentityClaimsTenantResolver : CurrentUserClaimsTenantResolver<IIdentityUserClaims>
    {
        public IdentityClaimsTenantResolver(IOptions<MultitenancyOptions> options,
            ICurrentUserManager<IIdentityUserClaims> currentUserManager) : base(options, currentUserManager)
        {
        }
    }
}
