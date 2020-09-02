using System.Collections.Generic;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Features;
using Microsoft.Extensions.Options;

namespace D3SK.NetCore.Infrastructure.Features
{
    public abstract class EntityProjectionQueryBase<TDomain, T, TQueryStore, TQueryContainer>
        : ProjectionStoreQueryBase<TDomain>, IEntityProjectionQuery<TDomain, T>
        where TDomain : IDomain
        where T : class, IEntityBase
        where TQueryStore : IQueryStore
        where TQueryContainer : IProjectionQueryContainerBase<T, TQueryStore>
    {
        protected readonly TQueryContainer QueryContainer;

        protected EntityProjectionQueryBase(IOptions<QueryOptions> queryOptions, TQueryContainer queryContainer)
            : base(queryOptions)
        {
            QueryContainer = queryContainer.NotNull(nameof(queryContainer));
        }

        public override async Task<IList<object>> HandleAsync(IDomainInstance<TDomain> domainInstance)
        {
            var items = await QueryContainer.GetAsync(this);
            return await ProcessItems(items);
        }

        protected virtual Task<IList<dynamic>> ProcessItems(IList<dynamic> items) => items.AsTask();
    }
}
