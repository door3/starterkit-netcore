namespace D3SK.NetCore.Common.Entities
{
    public interface ITenantEntityBase : IEntityBase
    {
        bool HasTenantId { get; }

        void SetTenantId(object tenantId);
    }

    public interface ITenantEntity : ITenantEntity<int>
    {
    }

    public interface ITenantEntity<TTenantKey> : ITenantEntityBase
    {
        TTenantKey TenantId { get; }

        void SetTenantId(TTenantKey tenantId);
    }
}
