using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SaasKit.Multitenancy;

namespace D3SK.NetCore.Infrastructure.Utilities
{
    public class TenantResolver : ITenantResolver<ResolvedTenant>
    {
        private readonly MultitenancyOptions _options;

        public TenantResolver(IOptions<MultitenancyOptions> options)
        {
            _options = options.Value.NotNull(nameof(options));
        }

        public Task<TenantContext<ResolvedTenant>> ResolveAsync(HttpContext context)
        {
            const int tenantId = 1;
            var tenant = new ResolvedTenant(tenantId);
            var tenantContext = new TenantContext<ResolvedTenant>(tenant);
            return tenantContext.AsTask();
        }
    }
}
