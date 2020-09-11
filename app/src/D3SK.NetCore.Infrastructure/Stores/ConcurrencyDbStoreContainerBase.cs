using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Stores;
using Microsoft.EntityFrameworkCore;

namespace D3SK.NetCore.Infrastructure.Stores
{
    public abstract class ConcurrencyDbStoreContainerBase<T, TStore, TDbStore>
        : ConcurrencyDbStoreContainerBase<T, int, TStore, TDbStore>
        where T : class, IEntity<int>
        where TStore : ICommandStore
        where TDbStore : DbContext, TStore
    {
        protected ConcurrencyDbStoreContainerBase(TDbStore store) : base(store)
        {
        }
    }

    public abstract class ConcurrencyDbStoreContainerBase<T, TKey, TStore, TDbStore>
        : DbStoreContainerBase<TStore, TDbStore>, IConcurrencyContainer<T, TStore>
        where T : class, IEntity<TKey>
        where TStore : ICommandStore
        where TDbStore : DbContext, TStore
    {
        protected ConcurrencyDbStoreContainerBase(TDbStore store) : base(store)
        {
        }

        public virtual async Task UpdateRowVersionAsync(T currentItem, T dbItem = null)
        {
            dbItem ??= await DbStore.FindAsync<T>(currentItem.Id);
            if (currentItem is IConcurrencyEntity concurrencyItem)
            {
                DbStore.Entry(dbItem).Property(nameof(concurrencyItem.RowVersion)).OriginalValue =
                    concurrencyItem.RowVersion;
            }
        }
    }
}
