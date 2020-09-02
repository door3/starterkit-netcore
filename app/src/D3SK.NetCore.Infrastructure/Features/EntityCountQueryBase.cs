using System.Collections.Generic;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Features;

namespace D3SK.NetCore.Infrastructure.Features
{
    public abstract class EntityCountQueryBase<TDomain, T, TQueryStore, TQueryContainer>
        : IEntityCountQuery<TDomain, T>
        where TDomain : IDomain
        where T : class, IEntityBase
        where TQueryStore : IQueryStore
        where TQueryContainer : IQueryContainerBase<T, TQueryStore>
    {
        protected readonly TQueryContainer QueryContainer;

        public IList<QueryFilter> Filters { get; set; } = new List<QueryFilter>();

        protected EntityCountQueryBase(TQueryContainer queryContainer)
        {
            QueryContainer = queryContainer.NotNull(nameof(queryContainer));
        }

        public virtual async Task<int> HandleAsync(IDomainInstance<TDomain> domainInstance) =>
            await QueryContainer.CountAsync(this);
    }
}