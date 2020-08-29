using System.Collections.Generic;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Domain;
using Microsoft.Extensions.Options;

namespace D3SK.NetCore.Infrastructure.Features
{
    public abstract class StoreQueryBase<TDomain, TResult> : IAsyncQueryFeature<TDomain, TResult>, IStoreQuery
        where TDomain : IDomain
    {
        protected QueryOptions QueryOptions { get; private set; }

        public IList<QueryFilter> Filters { get; set; } = new List<QueryFilter>();

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public string SortDirection { get; set; }

        public string SortField { get; set; }

        public bool TrackEntities { get; set; }

        public bool IncludeDeleted { get; set; }

        public string Includes { get; set; }

        protected StoreQueryBase(IOptions<QueryOptions> queryOptions)
        {
            QueryOptions = queryOptions.Value;
            PageSize = QueryOptions.DefaultPageSize;
            TrackEntities = QueryOptions.TrackEntities;
        }

        public abstract Task<TResult> HandleAsync(IDomainInstance<TDomain> domainInstance);
    }
}