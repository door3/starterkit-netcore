﻿using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Utilities
{
    public interface IUserClaimsBase
    {
        bool HasClaims { get; }

        T GetClaim<T>(string claim);
    }

    public interface IUserClaims : IUserClaims<int>
    {
    }

    public interface IUserClaims<out TUserKey> : IUserClaimsBase
    {
        TUserKey UserId { get; }
    }

    public interface ITenantUserClaims : ITenantUserClaims<int, int>
    {
    }

    public interface ITenantUserClaims<out TUserKey, out TTenantKey> : IUserClaims<TUserKey>
    {
        TTenantKey TenantId { get; }
    }

    public interface IIdentityUserClaims : IIdentityUserClaims<int, int>
    {
    }

    public interface IIdentityUserClaims<out TUserKey, out TTenantKey> : ITenantUserClaims<TUserKey, TTenantKey>
    {
    }
}
