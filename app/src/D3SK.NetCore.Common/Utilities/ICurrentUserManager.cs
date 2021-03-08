using System.Threading.Tasks;

namespace D3SK.NetCore.Common.Utilities
{
    public interface ICurrentUserManager
    {
        bool IsAuthenticated { get; }

        bool HasClaims { get; }

        Task<int> GetUserIdAsync();

        bool IsPrivateNetworkCall();
    }

    public interface ICurrentUserManager<TUserClaims> : ICurrentUserManager where TUserClaims : IUserClaimsBase
    {
        TUserClaims Claims { get; }

        void SetClaims(TUserClaims claims);
    }
}
