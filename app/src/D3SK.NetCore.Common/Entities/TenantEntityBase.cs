using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Common.Entities
{
    public abstract class TenantEntityBase : TenantEntityBase<int, int>
    {
        protected TenantEntityBase()
        {
        }

        protected TenantEntityBase(int id) : base(id)
        {
        }
    }

    public abstract class TenantEntityBase<TTenantKey, TKey> : EntityBase<TKey>, ITenantEntity<TTenantKey>
    {
        public TTenantKey TenantId { get; private set; }

        public bool HasTenantId => !TenantId.IsDefault(0);

        protected TenantEntityBase()
        {
        }

        protected TenantEntityBase(TKey id) : base(id)
        {
        }

        public virtual void SetTenantId(TTenantKey tenantId)
        {
            TenantId = tenantId;
        }

        public virtual void SetTenantId(object tenantId)
        {
            SetTenantId((TTenantKey)tenantId);
        }
    }
}
