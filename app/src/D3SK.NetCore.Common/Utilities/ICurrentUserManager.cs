namespace D3SK.NetCore.Common.Utilities
{
    public interface ICurrentUserManager<TUserClaims> where TUserClaims : IUserClaimsBase
    {
        bool IsAuthenticated { get; }

        bool HasClaims { get; }

        TUserClaims Claims { get; }

        void SetClaims(TUserClaims claims);
    }
}
