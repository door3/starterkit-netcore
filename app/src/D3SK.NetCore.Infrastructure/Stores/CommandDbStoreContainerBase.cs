using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Stores;
using Microsoft.EntityFrameworkCore;

namespace D3SK.NetCore.Infrastructure.Stores
{
    public abstract class CommandDbStoreContainerBase<T, TStore, TDbStore> : DbStoreContainerBase<TStore, TDbStore>,
        ICommandContainer<T, TStore>
        where T : class, IEntity<int>
        where TStore : ICommandStore
        where TDbStore : DbContext, TStore
    {
        protected CommandDbStoreContainerBase(TDbStore store) : base(store)
        {
        }

        public async Task AddAsync(T item)
        {
            item.NotNull(nameof(item));
            await DbStore.AddAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await DbStore.FindAsync<T>(id);
            DbStore.Remove(entity.NotNull(nameof(entity)));
        }

        public async Task UpdateAsync(T currentItem, T originalItem = null)
        {
            currentItem.NotNull(nameof(currentItem));
            var dbItem = await DbStore.FindAsync<T>(currentItem.Id);
            if (currentItem is IConcurrencyEntity concurrencyItem)
            {
                DbStore.Entry(dbItem).Property(nameof(concurrencyItem.RowVersion)).OriginalValue =
                    concurrencyItem.RowVersion;
            }

            DbStore.Entry(dbItem).State = EntityState.Modified;
        }
    }
}