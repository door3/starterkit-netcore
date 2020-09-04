using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using D3SK.NetCore.Common;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace D3SK.NetCore.Infrastructure.Stores
{
    public abstract class TenantDbStoreBase : DomainDbStoreBase
    {
        protected ITenantManager TenantManager { get; }

        public int? TenantId => TenantManager.TenantId;
        
        protected TenantDbStoreBase(
            DbContextOptions options,
            IDomainInstance domainInstance,
            ITenantManager tenantManager,
            IClock currentClock)
            : base(options, domainInstance, currentClock)
        {
            TenantManager = tenantManager.NotNull(nameof(tenantManager));
        }

        protected override async Task<int> SaveChangesCoreAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SetTenantIdForAddedEntities();

            return await base.SaveChangesCoreAsync(cancellationToken);
        }

        protected virtual void SetTenantIdForAddedEntities()
        {
            var entities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity).OfType<ITenantEntityBase>();

            foreach (var entity in entities)
            {
                if (!entity.HasTenantId)
                {
                    entity.SetTenantId(TenantId);
                }
            }
        }

        protected EntityTypeBuilder<T> ApplyDeletedAndTenantIdFilter<T>(EntityTypeBuilder<T> source)
            where T : class, ISoftDeleteEntity, ITenantEntity<int>
        {
            return source.HasQueryFilter(x => !x.IsDeleted && x.TenantId == TenantId);
        }

        protected EntityTypeBuilder<T> ApplyDeletedAndTenantAllowNullIdFilter<T>(EntityTypeBuilder<T> source)
            where T : class, ISoftDeleteEntity, ITenantEntity<int?>
        {

            return source.HasQueryFilter(x => !x.IsDeleted && (x.TenantId == null || x.TenantId == TenantId));
        }

        protected EntityTypeBuilder<T> ApplyTenantIdFilter<T>(EntityTypeBuilder<T> source)
            where T : class, ITenantEntity<int>
        {
            return source.HasQueryFilter(x => x.TenantId == TenantId);
        }

        protected EntityTypeBuilder<T> ApplyTenantAllowNullIdFilter<T>(EntityTypeBuilder<T> source)
            where T : class, ITenantEntity<int?>
        {
            return source.HasQueryFilter(x => x.TenantId == null || x.TenantId == TenantId);
        }
    }
}
