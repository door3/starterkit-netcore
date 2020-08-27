using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Stores;

namespace D3SK.NetCore.Domain.Stores
{
    public interface IStoreQueryFeature : IStoreQuery, IQueryFeature
    {
    }

    public interface IProjectionStoreQueryFeature : IProjectionStoreQuery, IStoreQueryFeature
    {
    }
}
