using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Domain.Stores;

namespace D3SK.NetCore.Domain.Features
{
    public interface IEntityUpdateCommand<T> : IStoreCommandFeature
        where T : class, IEntityBase
    {
        T OriginalItem { get; set; }

        T CurrentItem { get; set; }
    }

    public interface IEntityUpdateCommand<TDomain, T> : IAsyncCommandFeature<TDomain>, IEntityUpdateCommand<T>
        where TDomain : IDomain
        where T : class, IEntityBase
    {
    }
}
