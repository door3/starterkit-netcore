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
                await HandleDomainEvents();
            }

            var result = await SaveChangesCoreAsync(cancellationToken);

            if (IsInTransaction)
            {
                await HandleDomainEvents();
                result += await SaveChangesCoreAsync(cancellationToken);
            }

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

            addedEntities.ForEach(entity => entity.AddDomainEvent(new EntityAddedDomainEvent(entity)));

            deletedEntities.ForEach(entity =>
            {
                var entityId = this.GetPrimaryKeys(entity);
                entity.AddDomainEvent(new EntityDeletedDomainEvent(entity, entityId));
            });

            updatedEntities.ForEach(entity =>
            {
                var entityId = this.GetPrimaryKeyObject(entity);
                var changes = Entry(entity).GetPropertyChanges();
                entity.AddDomainEvent(new EntityUpdatedDomainEvent(entity, entityId, changes));
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
        
        protected virtual async Task HandleDomainEvents()
        {
            var domainEntities = GetDomainEntities();
            var domainEvents = GetDomainEvents(domainEntities);
            while (domainEvents.Any())
            {
                ClearDomainEvents(domainEntities);

                foreach (var domainEvent in domainEvents)
                {
                    await DomainInstance.PublishEventAsync(domainEvent);
                }

                domainEntities = GetDomainEntities();
                domainEvents = GetDomainEvents(domainEntities);
            }
        }

        protected IList<IDomainEvent> GetDomainEvents(IList<IDomainEntity> domainEntities)
        {
            return domainEntities.SelectMany(x => x.DomainEvents).ToList();
        }

        protected void ClearDomainEvents(IList<IDomainEntity> domainEntities)
        {
            domainEntities.ForEach(x => x.ClearDomainEvents());
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
