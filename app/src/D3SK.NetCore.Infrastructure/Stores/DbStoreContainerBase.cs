using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Stores;
using Microsoft.EntityFrameworkCore;

namespace D3SK.NetCore.Infrastructure.Stores
{

    public abstract class DbStoreContainerBase<TStore, TDbStore> : StoreContainerBase<TStore>, IQueryContainer<TStore>
        where TStore : IQueryStore
        where TDbStore : DbContext, TStore
    {
        protected readonly TDbStore DbStore;

        protected DbStoreContainerBase(TDbStore store) : base(store)
        {
            DbStore = store;
        }
    }

}
