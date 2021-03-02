using System.Threading.Tasks;

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

    public interface IUserClaims<TUserKey> : IUserClaimsBase
    {
        Task<TUserKey> GetUserIdAsync();
    }

    public interface ITenantUserClaims : ITenantUserClaims<int, int>, IUserClaims
    {
    }

    public interface ITenantUserClaims<TUserKey, TTenantKey> : IUserClaims<TUserKey>
    {
        Task<TTenantKey> GetTenantIdAsync();
    }
}
