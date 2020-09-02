using System.ComponentModel.DataAnnotations;
using D3SK.NetCore.Common.Entities;

namespace D3SK.NetCore.Domain.Entities
{
    public abstract class DomainConcurrencyEntityBase : DomainConcurrencyEntityBase<int, int, int>
    {
        protected DomainConcurrencyEntityBase()
        {
        }

        protected DomainConcurrencyEntityBase(int id) : base(id)
        {
        }
    }

    public abstract class DomainConcurrencyEntityBase<TTenantKey, TKey, TUserKey> : DomainEntityBase<TTenantKey, TKey, TUserKey>, IConcurrencyEntity
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        [Timestamp]
        public byte[] RowVersion { get; private set; }

        protected DomainConcurrencyEntityBase()
        {
        }

        protected DomainConcurrencyEntityBase(TKey id) : base(id)
        {
        }
    }

    public abstract class DomainConcurrencyEntityBase<TKey, TUserKey> : DomainEntityBase<TKey, TUserKey>, IConcurrencyEntity
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        [Timestamp]
        public byte[] RowVersion { get; private set; }

        protected DomainConcurrencyEntityBase()
        {
        }

        protected DomainConcurrencyEntityBase(TKey id) : base(id)
        {
        }
    }
}
