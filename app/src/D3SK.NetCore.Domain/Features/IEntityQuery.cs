using System.Collections.Generic;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Domain.Stores;

namespace D3SK.NetCore.Domain.Features
{
    public interface IEntityQuery<T> : IStoreQueryFeature, IAllowNonOwnerPermissionQuery
    {
    }

    public interface IEntityQuery<TDomain, T> : IAsyncQueryFeature<TDomain, IList<T>>, IEntityQuery<T>
        where TDomain : IDomain
        where T : class, IEntityBase
    {
    }
}
