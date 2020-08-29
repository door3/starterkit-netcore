using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Domain.Events;

namespace D3SK.NetCore.Domain.Entities
{
    public abstract class DeletableDomainEntityBase : DeletableDomainEntityBase<int, int, int>
    {
        protected DeletableDomainEntityBase()
        {
        }

        protected DeletableDomainEntityBase(int id) : base(id)
        {
        }
    }

    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public abstract class DeletableDomainEntityBase<TTenantKey, TKey, TUserKey> : TenantEntityBase<TTenantKey, TKey>,
        IDomainEntity, IAuditEntity<TUserKey>
    {
        [JsonIgnore] public IList<IDomainEvent> DomainEvents { get; } = new List<IDomainEvent>();

        public DateTimeOffset CreatedDate { get; private set; }

        public TUserKey CreatedByUser { get; private set; }

        public DateTimeOffset LastModifiedDate { get; private set; }

        public TUserKey LastModifiedByUser { get; private set; }

        protected DeletableDomainEntityBase()
        {
        }

        protected DeletableDomainEntityBase(TKey id) : base(id)
        {
        }

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            DomainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            DomainEvents.Clear();
        }

        public virtual void OnAdded(DateTimeOffset createdDate, TUserKey createdByUser)
        {
            if (CreatedDate != default) return;

            CreatedDate = createdDate;
            CreatedByUser = createdByUser;
            LastModifiedDate = createdDate;
            LastModifiedByUser = createdByUser;
        }

        public virtual void OnUpdated(DateTimeOffset lastModifiedDate, TUserKey lastModifiedByUser)
        {
            LastModifiedDate = lastModifiedDate;
            LastModifiedByUser = lastModifiedByUser;
        }

        public virtual void OnAdded(DateTimeOffset createdDate, object createdByUser)
            => OnAdded(createdDate, createdByUser is TUserKey userId ? userId : default);

        public virtual void OnUpdated(DateTimeOffset lastModifiedDate, object lastModifiedByUser)
            => OnAdded(lastModifiedDate, lastModifiedByUser is TUserKey userId ? userId : default);
    }
}
