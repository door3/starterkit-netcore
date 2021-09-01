using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Utilities;
using Microsoft.AspNetCore.Http;

namespace D3SK.NetCore.Api.Utilities
{
    public class HttpCurrentUserManager<TClaims> : ICurrentUserManager<TClaims> where TClaims : IUserClaimsBase
    {
        protected readonly IHttpContextAccessor HttpContextAccessor;

      protected HttpContext HttpContext => HttpContextAccessor.HttpContext;

        public bool IsAuthenticated => HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public bool HasClaims => IsAuthenticated && Claims.HasClaims;

        public virtual Task<int> GetUserIdAsync() => Claims.GetClaim<int>(UserClaims.ClaimTypes.UserId).AsTask();

        public TClaims Claims { get; private set; }

        public HttpCurrentUserManager(IHttpContextAccessor httpContextAccessor, TClaims claims)
        {
            HttpContextAccessor = httpContextAccessor.NotNull(nameof(httpContextAccessor));
            Claims = claims.NotNull(nameof(claims));
        }

        public void SetClaims(TClaims claims)
        {
            Claims = claims;
        }

        public virtual bool IsPrivateNetworkCall() => false;
    }
}
