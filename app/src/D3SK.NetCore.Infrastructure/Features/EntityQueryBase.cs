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
    public abstract class EntityQueryBase<TDomain, T, TQueryStore, TQueryContainer>
        : StoreQueryBase<TDomain, IList<T>>, IEntityQuery<TDomain, T>
        where TDomain : IDomain
        where T : class, IEntityBase
        where TQueryStore : IQueryStore
        where TQueryContainer : IQueryContainerBase<T, TQueryStore>
    {
        protected readonly TQueryContainer QueryContainer;

        protected EntityQueryBase(IOptions<QueryOptions> queryOptions, TQueryContainer queryContainer)
            : base(queryOptions)
        {
            QueryContainer = queryContainer.NotNull(nameof(queryContainer));
        }

        public override async Task<IList<T>> HandleAsync(IDomainInstance<TDomain> domainInstance)
        {
            var items = await QueryContainer.GetAsync(this);
            return await ProcessItems(items);
        }

        protected virtual Task<IList<T>> ProcessItems(IList<T> items) => items.AsTask();
    }
}