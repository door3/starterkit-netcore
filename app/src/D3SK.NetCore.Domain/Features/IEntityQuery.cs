using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Domain.Stores;

namespace D3SK.NetCore.Domain.Features
{
    public interface IEntityQuery<T> : IStoreQueryFeature
    {
    }

    public interface IEntityQuery<TDomain, T> : IAsyncQueryFeature<TDomain, IList<T>>, IEntityQuery<T>
        where TDomain : IDomain
        where T : class, IEntityBase
    {
    }
}
