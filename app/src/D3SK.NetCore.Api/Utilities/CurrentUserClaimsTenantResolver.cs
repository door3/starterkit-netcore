﻿using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SaasKit.Multitenancy;

namespace D3SK.NetCore.Api.Utilities
{
    public class CurrentUserClaimsTenantResolver<TUserClaims> : ITenantResolver<ResolvedTenant>
        where TUserClaims : ITenantUserClaims
    {
        private readonly MultitenancyOptions _options;

        private readonly ICurrentUserManager<TUserClaims> _currentUserManager;

        public CurrentUserClaimsTenantResolver(IOptions<MultitenancyOptions> options,
            ICurrentUserManager<TUserClaims> currentUserManager)
        {
            _options = options.Value.NotNull(nameof(options));
            _currentUserManager = currentUserManager.NotNull(nameof(currentUserManager));
        }

        public Task<TenantContext<ResolvedTenant>> ResolveAsync(HttpContext context)
        {
            var tenantId = _currentUserManager.HasClaims ? _currentUserManager.Claims.TenantId : 1;
            var tenant = new ResolvedTenant(tenantId);
            var tenantContext = new TenantContext<ResolvedTenant>(tenant);
            return tenantContext.AsTask();
        }
    }
}