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
    public class UserClaims : UserClaims<int>, IUserClaims
    {
        public UserClaims(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
    }

    public class UserClaims<TUserKey> : IUserClaims<TUserKey>
    {
        public class ClaimTypes
        {
            public const string UserId = "userId";
            public const string TenantId = "tenantId";
        }

        protected readonly IHttpContextAccessor HttpContextAccessor;

        protected HttpContext HttpContext => HttpContextAccessor.HttpContext;

        protected IEnumerable<Claim> Claims => HttpContext?.User?.Claims;

        public bool HasClaims => HttpContext?.User?.Claims?.Any() ?? false;

        public TUserKey UserId => GetClaim<TUserKey>(ClaimTypes.UserId);

        public UserClaims(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor.NotNull(nameof(httpContextAccessor));
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
