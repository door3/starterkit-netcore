using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;

namespace D3SK.NetCore.Common.Stores
{
    public interface IUpdateStrategy
    {
        Task AddEntityAsync<T>(T item, Func<EntityAddedEventArgs<T>, Task> onAddComplete = null,
            AddEntityOptions options = null)
            where T : class, IEntityBase;

        Task DeleteEntityAsync<T>(T item, Func<EntityDeletedEventArgs<T>, Task> onDeleteComplete = null,
            DeleteEntityOptions options = null)
            where T : class, IEntityBase;

        Task UpdateEntityAsync<T>(
            T currentItem,
            T originalItem = null,
            T dbItem = null,
            Func<EntityPropertyUpdatedEventArgs<T>, Task> onPropertyChanged = null,
            Func<EntityUpdatedEventArgs<T>, Task> onUpdateComplete = null,
            UpdateEntityOptions options = null)
            where T : class, IEntityBase;

        Task UpdateCollectionAsync<T, TKey>(
            IEnumerable<T> currentItems,
            IEnumerable<T> originalItems,
            IEnumerable<T> dbItems,
            Func<T, Task> onAddItem = null,
            Func<T, T, T, Task> onUpdateItem = null,
            Func<T, Task> onDeleteItem = null,
            Func<T, TKey> getItemId = null,
            Func<TKey, T> findItem = null,
            UpdateCollectionOptions options = null)
            where T : class, IEntityBase;
    }
}
