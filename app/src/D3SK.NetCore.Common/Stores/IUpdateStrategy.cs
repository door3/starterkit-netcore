using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;

namespace D3SK.NetCore.Common.Stores
{
    public interface IUpdateStrategy
    {
        Task AddEntityAsync<T>(T item, Func<EntityAddedEventArgs<T>, Task> onAddComplete = null)
            where T : class, IEntityBase;

        Task DeleteEntityAsync<T>(T item, Func<EntityDeletedEventArgs<T>, Task> onDeleteComplete = null)
            where T : class, IEntityBase;

        Task UpdateEntityAsync<T>(
            T currentItem,
            T originalItem = null,
            T dbItem = null,
            Func<EntityPropertyUpdatedEventArgs<T>, Task> onPropertyChanged = null,
            Func<EntityUpdatedEventArgs<T>, Task> onUpdateComplete = null,
            bool updateNestedEntitiesAndCollections = false,
            Func<T, object> getItemId = null)
            where T : class, IEntityBase;

        Task UpdateCollectionAsync<T, TKey>(
            IEnumerable<T> currentItems,
            IEnumerable<T> originalItems,
            IEnumerable<T> dbItems,
            Func<T, TKey> getItemId = null,
            Func<TKey, T> findItem = null,
            Func<T, Task> onAddItem = null,
            Func<T, T, T, Task> onUpdateItem = null,
            Func<T, Task> onDeleteItem = null)
            where T : class, IEntityBase;

        Task<bool> UpdateCompositeEntityAsync<T, TComposite>(T currentRootItem, string namePrefix,
            TComposite currentItem,
            TComposite originalItem = null, TComposite dbItem = null,
            Func<EntityPropertyUpdatedEventArgs<T>, Task> onPropertyChanged = null)
            where T : class, IEntityBase
            where TComposite : class, ICompositeEntity;
    }
}