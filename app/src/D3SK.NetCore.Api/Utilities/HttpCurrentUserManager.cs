using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Utilities;
using Microsoft.AspNetCore.Http;

namespace D3SK.NetCore.Api.Utilities
{
    public class HttpCurrentUserManager<TClaims> : ICurrentUserManager<TClaims> where TClaims : IIdentityUserClaims
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected HttpContext HttpContext => _httpContextAccessor.HttpContext;

        public bool IsAuthenticated => HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public bool HasClaims => IsAuthenticated && Claims.HasClaims;

        public TClaims Claims { get; private set; }

        public HttpCurrentUserManager(IHttpContextAccessor httpContextAccessor, TClaims claims)
        {
            _httpContextAccessor = httpContextAccessor.NotNull(nameof(httpContextAccessor));
            Claims = claims.NotNull(nameof(claims));
        }

        public void SetClaims(TClaims claims)
        {
            Claims = claims;
        }
    }
}
