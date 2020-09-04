using D3SK.NetCore.Common.Utilities;
using Microsoft.AspNetCore.Http;

namespace D3SK.NetCore.Api.Utilities
{
    public class TenantUserClaims : TenantUserClaims<int, int>, ITenantUserClaims
    {
        public TenantUserClaims(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
    }

    public class TenantUserClaims<TUserKey, TTenantKey> : UserClaims<TUserKey>,
        ITenantUserClaims<TUserKey, TTenantKey>
    {
        public TTenantKey TenantId => GetClaim<TTenantKey>(ClaimTypes.TenantId);

        public TenantUserClaims(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
    }
}
