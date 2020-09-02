using D3SK.NetCore.Common.Entities;

namespace D3SK.NetCore.Domain.Entities
{
    public abstract class DomainEntityBase : DomainEntityBase<int, int, int>
    {
        protected DomainEntityBase()
        {
        }

        protected DomainEntityBase(int id) : base(id)
        {
        }
    }

    public abstract class DomainEntityBase<TTenantKey, TKey, TUserKey> : DeletableDomainEntityBase<TTenantKey, TKey, TUserKey>, ISoftDeleteEntity
    {
        public bool IsDeleted { get; private set; }

        protected DomainEntityBase()
        {
        }

        protected DomainEntityBase(TKey id) : base(id)
        {
        }

        public void SetDeleted(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }
    }

    public abstract class DomainEntityBase<TKey, TUserKey> : DeletableDomainEntityBase<TKey, TUserKey>, ISoftDeleteEntity
    {
        public bool IsDeleted { get; private set; }

        protected DomainEntityBase()
        {
        }

        protected DomainEntityBase(TKey id) : base(id)
        {
        }

        public void SetDeleted(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }
    }
}
