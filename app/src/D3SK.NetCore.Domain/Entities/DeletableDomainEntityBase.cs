using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;
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

    public abstract class DeletableDomainEntityBase<TTenantKey, TKey, TUserKey> : DeletableDomainEntityBase<TKey, TUserKey>,
        ITenantEntity<TTenantKey>, IAuditEntity<TUserKey>
    {
        public TTenantKey TenantId { get; private set; }

        public bool HasTenantId => !TenantId.IsDefault(0);

        protected DeletableDomainEntityBase()
        {
        }

        protected DeletableDomainEntityBase(TKey id) : base(id)
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
    
    public abstract class DeletableDomainEntityBase<TKey, TUserKey> : EntityBase<TKey>,
        IDomainEntity, IAuditEntity<TUserKey>
    {
        [JsonIgnore] public IList<IDomainEventBase> DomainEvents { get; } = new List<IDomainEventBase>();

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

        public void AddDomainEvent(IDomainEventBase domainEvent)
        {
            DomainEvents.Add(domainEvent);
        }

        public void ClearAllDomainEvents()
        {
            DomainEvents.Clear();
        }

        public void ClearDomainEvents<TEvent>() where TEvent : IDomainEventBase
        {
            DomainEvents.Where(x => x.GetType().ImplementsInterface<TEvent>()).ForEach(x => DomainEvents.Remove(x));
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
            => OnUpdated(lastModifiedDate, lastModifiedByUser is TUserKey userId ? userId : default);
    }
}
