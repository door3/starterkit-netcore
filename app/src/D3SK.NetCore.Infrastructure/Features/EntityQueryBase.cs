using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Features;
using D3SK.NetCore.Infrastructure.Extensions;
using Microsoft.Extensions.Options;

namespace D3SK.NetCore.Infrastructure.Features
{
    public abstract class EntityQueryBase<TDomain, T, TQueryStore, TQueryContainer>
        : StoreQueryBase<TDomain, IList<T>>, IEntityQuery<TDomain, T>
        where TDomain : IDomain
        where T : class, IEntity<int>
        where TQueryStore : IQueryStore
        where TQueryContainer : IQueryContainer<T, int, TQueryStore>
    {
        protected readonly TQueryContainer QueryContainer;

        protected EntityQueryBase(IOptions<QueryOptions> queryOptions, TQueryContainer queryContainer)
            : base(queryOptions)
        {
            QueryContainer = queryContainer.NotNull(nameof(queryContainer));
        }

        public override async Task<IList<T>> HandleAsync(IDomainInstance<TDomain> domainInstance)
        {
            if (this.IsSingleItemIdFilter())
            {
                var idValue = Filters.FirstOrDefault()?.Value?.ToString();

                // NOTE: Support legacy behavior on FE
                if (!int.TryParse(idValue, out var id))
                {
                    return await HandleCoreAsync();
                }

                var item = await QueryContainer.GetAsync(id, Includes ?? StoreQueryIncludes.Full);

                if (domainInstance.Domain.DomainOptions.UseAuthorization && NonOwnerPermissionId > 0)
                {
                    if (item is IAuditEntity<int> auditEntity)
                    {
                        var userId = await domainInstance.GetCurrentUserManager().GetUserIdAsync();
                        if (auditEntity.CreatedByUser != userId && !CurrentUserHasNonOwnerPermission)
                        {
                            domainInstance.ExceptionManager.ThrowException(new UnauthorizedAccessException(), 403);
                        }
                    }
                }

                return await ProcessItems(new[] { item });
            }

            return await HandleCoreAsync();
        }

        protected virtual Task<IList<T>> ProcessItems(IList<T> items) => items.AsTask();

        private async Task<IList<T>> HandleCoreAsync()
        {
            var items = await QueryContainer.GetAsync(this);
            return await ProcessItems(items);
        }
    }
}
