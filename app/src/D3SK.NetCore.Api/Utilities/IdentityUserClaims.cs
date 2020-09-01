using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Utilities;
using Microsoft.AspNetCore.Http;

namespace D3SK.NetCore.Api.Utilities
{
    public class IdentityUserClaims : IIdentityUserClaims
    {
        public class ClaimTypes
        {
            public const string UserId = "userId";
            public const string TenantId = "tenantId";
        }

        private readonly IHttpContextAccessor _httpContextAccessor;

        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;
        
        protected IEnumerable<Claim> Claims => HttpContext?.User?.Claims;

        public bool HasClaims => HttpContext?.User?.Claims?.Any() ?? false;

        public int UserId => GetClaim<int>(ClaimTypes.UserId);

        public int TenantId => GetClaim<int>(ClaimTypes.TenantId);

        public IdentityUserClaims(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor.NotNull(nameof(httpContextAccessor));
        }

        public T GetClaim<T>(string claimType)
        {
            if (!HasClaims || !HttpContext.User.HasClaim(ClaimCheck))
            {
                return default;
            }

            return (T)Convert.ChangeType(HttpContext.User.FindFirst(ClaimCheck)?.Value, typeof(T));

            bool ClaimCheck(Claim x) => x.Type.IsSame(claimType);
        }
    }
}
