using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Stores;
using Microsoft.EntityFrameworkCore;

namespace D3SK.NetCore.Infrastructure.Stores
{
    public abstract class CommandDbStoreContainerBase<T, TStore, TDbStore> :
        CommandDbStoreContainerBase<T, int, TStore, TDbStore>
        where T : class, IEntity<int>
        where TStore : ICommandStore
        where TDbStore : DbContext, TStore
    {
        protected CommandDbStoreContainerBase(TDbStore store) : base(store)
        {
        }
    }

    public abstract class CommandDbStoreContainerBase<T, TKey, TStore, TDbStore> : 
        ConcurrencyDbStoreContainerBase<T, TKey, TStore, TDbStore>, ICommandContainer<T, TKey, TStore>
        where T : class, IEntity<TKey>
        where TStore : ICommandStore
        where TDbStore : DbContext, TStore
    {
        protected CommandDbStoreContainerBase(TDbStore store) : base(store)
        {
        }

        public virtual async Task<T> FindAsync(TKey id)
        {
            var item = await DbStore.Set<T>().FindAsync(id);
            return item != null ? await LoadRelationsAsync(item) : null;
        }

        public virtual async Task AddAsync(T item)
        {
            item.NotNull(nameof(item));
            await DbStore.AddAsync(item);
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var entity = await DbStore.FindAsync<T>(id);
            DbStore.Remove(entity.NotNull(nameof(entity)));
        }

        public virtual async Task UpdateAsync(T currentItem, T originalItem = null)
        {
            currentItem.NotNull(nameof(currentItem));
            await UpdateRowVersionAsync(currentItem);
        }
        
        protected virtual Task<T> LoadRelationsAsync(T item)
        {
            return item.AsTask();
        }
    }
}