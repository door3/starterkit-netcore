using System.Collections.Generic;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Domain;
using Microsoft.Extensions.Options;

namespace D3SK.NetCore.Infrastructure.Features
{
    public abstract class ProjectionStoreQueryBase<TDomain> : StoreQueryBase<TDomain, IList<object>>,
        IProjectionStoreQuery
        where TDomain : IDomain
    {
        public IList<string> SelectProperties { get; set; } = new List<string>();

        public bool Distinct { get; set; }

        protected ProjectionStoreQueryBase(IOptions<QueryOptions> queryOptions) : base(queryOptions)
        {
        }
    }
}
