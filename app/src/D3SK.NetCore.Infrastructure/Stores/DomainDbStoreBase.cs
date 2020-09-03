using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using D3SK.NetCore.Common;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Entities;
using D3SK.NetCore.Domain.Events;
using D3SK.NetCore.Domain.Events.EntityEvents;
using D3SK.NetCore.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace D3SK.NetCore.Infrastructure.Stores
{
    public abstract class DomainDbStoreBase : DbContext, ITransactionStore
    {
        protected IDomainInstance DomainInstance { get; }

        protected IClock CurrentClock { get; }
        
        public IStoreTransaction CurrentTransaction { get; protected set; }

        public bool IsInTransaction { get; protected set; }

        protected DomainDbStoreBase(
            DbContextOptions options, 
            IDomainInstance domainInstance,
            IClock currentClock)
            : base(options)
        {
            DomainInstance = domainInstance.NotNull(nameof(domainInstance));
            CurrentClock = currentClock.NotNull(nameof(currentClock));
        }
        
        protected virtual void OnTransactionCommitted(object sender, EventArgs e)
        {
            IsInTransaction = false;
            CurrentTransaction = null;
        }

        protected virtual void OnTransactionRolledBack(object sender, EventArgs e)
        {
            IsInTransaction = false;
            CurrentTransaction = null;
        }

        public virtual async Task<IStoreTransaction> BeginTransactionAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (IsInTransaction) return CurrentTransaction;

            IsInTransaction = true;
            var dbTransaction = await Database.BeginTransactionAsync(cancellationToken);
            CurrentTransaction = new StoreDbTransaction(dbTransaction);
            CurrentTransaction.Committed += OnTransactionCommitted;
            CurrentTransaction.RolledBack += OnTransactionRolledBack;
            
            return CurrentTransaction;
        }

        public virtual IStoreTransaction UseTransaction(IStoreTransaction transaction)
        {
            if (IsInTransaction) return CurrentTransaction;

            IsInTransaction = true;
            CurrentTransaction = transaction.NotNull(nameof(transaction));
            CurrentTransaction.Committed += OnTransactionCommitted;
            CurrentTransaction.RolledBack += OnTransactionRolledBack;
            Database.UseTransaction((DbTransaction)transaction.GetTransaction());

            return transaction;
        }

        public virtual void CommitTransaction()
        {
            CurrentTransaction.Commit();
        }

        public virtual async Task RunCommandAsync(string command, params object[] parameters)
        {
            await Database.ExecuteSqlRawAsync(command, parameters);
        }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (!IsInTransaction)
            {
                await HandleDomainEventsAsync<IDomainEvent>();
            }

            var result = await SaveChangesCoreAsync(cancellationToken);

            if (IsInTransaction)
            {
                do
                {
                    await HandleDomainEventsAsync<IDomainEvent>();
                    result += await SaveChangesCoreAsync(cancellationToken);
                } while (HasDomainEvents<IDomainEvent>());
            }

            await HandleDomainEventsAsync<IBusEvent>();
            ClearAllDomainEvents();

            return result;
        }

        protected virtual async Task<int> SaveChangesCoreAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();

            SetAuditEntityDetails();
            AddDomainEventsToEntities();
            SetIsDeletedForDeletedEntities();
            
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected virtual void AddDomainEventsToEntities()
        {
            var addedEntities = GetDomainEntities(EntityState.Added).ToList();
            var deletedEntities = GetDomainEntities(EntityState.Deleted).ToList();
            var updatedEntities = GetDomainEntities(EntityState.Modified).ToList();

            addedEntities.ForEach(entity => entity.AddDomainEvent(new EntityAddedBusEvent(entity)));

            deletedEntities.ForEach(entity =>
            {
                var entityId = this.GetPrimaryKeys(entity);
                entity.AddDomainEvent(new EntityDeletedBusEvent(entity, entityId));
            });

            updatedEntities.ForEach(entity =>
            {
                var entityId = this.GetPrimaryKeyObject(entity);
                var changes = Entry(entity).GetPropertyChanges();
                entity.AddDomainEvent(new EntityUpdatedBusEvent(entity, entityId, changes));
            });
        }

        protected virtual void SetIsDeletedForDeletedEntities()
        {
            var entities = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted)
                .Select(x => x.Entity).OfType<ISoftDeleteEntity>();

            foreach (var entity in entities)
            {
                Entry(entity).State = EntityState.Modified;
                entity.SetDeleted(true);
            }
        }

        protected virtual void SetAuditEntityDetails()
        {
            var added = ChangeTracker.Entries().Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity).OfType<IAuditEntityBase>();

            var modified = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified)
                .Select(x => x.Entity).OfType<IAuditEntityBase>();

            // TODO: set user id for audit entities

            foreach (var entity in added)
            {
                entity.OnAdded(CurrentClock.UtcNow, null);
            }

            foreach (var entity in modified)
            {
                entity.OnUpdated(CurrentClock.UtcNow, null);
            }
        }

        protected bool HasDomainEvents<TEvent>() where TEvent : IDomainEventBase
        {
            var domainEntities = GetDomainEntities();
            var domainEvents = GetDomainEvents<TEvent>(domainEntities);

            return domainEvents.Any();
        }
        
        protected virtual async Task HandleDomainEventsAsync<TEvent>() where TEvent : IDomainEventBase
        {
            var domainEntities = GetDomainEntities();
            var domainEvents = GetDomainEvents<TEvent>(domainEntities);
            while (domainEvents.Any())
            {
                ClearDomainEvents<TEvent>(domainEntities);

                foreach (var domainEvent in domainEvents)
                {
                    await DomainInstance.PublishEventAsync(domainEvent);
                }

                domainEntities = GetDomainEntities();
                domainEvents = GetDomainEvents<TEvent>(domainEntities);
            }
        }

        protected IList<TEvent> GetDomainEvents<TEvent>(IList<IDomainEntity> domainEntities) where TEvent : IDomainEventBase
        {
            return domainEntities.SelectMany(x => x.DomainEvents).OfType<TEvent>().ToList();
        }

        protected void ClearDomainEvents<TEvent>(IList<IDomainEntity> domainEntities) where TEvent : IDomainEventBase
        {
            domainEntities.ForEach(x => x.ClearDomainEvents<TEvent>());
        }

        protected void ClearAllDomainEvents(IList<IDomainEntity> domainEntities = null)
        {
            domainEntities ??= GetDomainEntities();
            domainEntities.ForEach(x => x.ClearAllDomainEvents());
        }

        protected IList<IDomainEntity> GetDomainEntities(EntityState? state = null)
        {
            return ChangeTracker.Entries().Where(x => !state.HasValue || x.State == state.Value)
                .Select(x => x.Entity).OfType<IDomainEntity>().ToList();
        }

        protected EntityTypeBuilder<T> ApplyDeletedFilter<T>(EntityTypeBuilder<T> source)
            where T : class, ISoftDeleteEntity
        {
            return source.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
