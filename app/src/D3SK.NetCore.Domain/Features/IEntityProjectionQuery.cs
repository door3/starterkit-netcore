using System.Collections.Generic;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Domain.Stores;

namespace D3SK.NetCore.Domain.Features
{
    public interface IEntityProjectionQuery : IProjectionStoreQueryFeature
    {
    }

    public interface IEntityProjectionQuery<TDomain, T> : IAsyncQueryFeature<TDomain, IList<object>>,
        IProjectionStoreQueryFeature, IAllowNonOwnerPermissionQuery
        where TDomain : IDomain
        where T : class, IEntityBase
    {
    }
}