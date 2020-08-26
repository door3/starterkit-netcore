using D3SK.NetCore.Common.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Infrastructure.Stores
{
    public class OptimisticUpdateStrategy : IUpdateStrategy
    {
        public static void SetNullOnAdd(object obj)
        {
            var entityType = obj.GetType();
            var properties = entityType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(p => new { Property = p, Attribute = p.GetCustomAttribute<UpdateStrategyAttribute>() })
                .Where(p => p.Attribute != null && p.Attribute.NullOnAdd)
                .ToList();
            properties.ForEach(p => p.Property.SetValue(obj, null, null));

            var collections = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(c => c.PropertyType.IsGenericType &&
                            c.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                .ToList();
            collections.ForEach(c =>
            {
                var list = (IEnumerable)c.GetValue(obj);
                foreach (var i in list)
                {
                    SetNullOnAdd(i);
                }
            });
        }

        public virtual async Task AddEntityAsync<T>(T item, Func<EntityAddedEventArgs<T>, Task> onAddComplete = null)
            where T : class, IEntityBase
        {
            SetNullOnAdd(item);

            if (onAddComplete == null) return;

            var eventArgs = new EntityAddedEventArgs<T>(item);
            await onAddComplete(eventArgs);
        }

        public virtual async Task DeleteEntityAsync<T>(T item, Func<EntityDeletedEventArgs<T>, Task> onDeleteComplete = null)
            where T : class, IEntityBase
        {
            if (onDeleteComplete == null) return;

            var eventArgs = new EntityDeletedEventArgs<T>(item);
            await onDeleteComplete(eventArgs);
        }

        public virtual async Task UpdateCollectionAsync<T, TKey>(
            IEnumerable<T> currentItems,
            IEnumerable<T> originalItems,
            IEnumerable<T> dbItems,
            Func<T, TKey> getItemId = null,
            Func<TKey, T> findItem = null,
            Func<T, Task> onAddItem = null,
            Func<T, T, T, Task> onUpdateItem = null,
            Func<T, Task> onDeleteItem = null)
            where T : class, IEntityBase
        {
            currentItems = currentItems.ToList();
            dbItems = dbItems.ToList();
            originalItems = originalItems?.ToList() ?? dbItems;

            bool IdsAreEqual(TKey idA, TKey idB) => Equals(idA, idB);

            getItemId ??= (item => (TKey) GetEntityId(item));
            findItem ??= (id =>
                !Equals(id, default(TKey))
                    ? dbItems.SingleOrDefault(x => IdsAreEqual(id, getItemId(x)))
                    : null);

            foreach (var item in currentItems)
            {
                var itemId = getItemId(item);
                var dbItem = findItem(itemId);
                if (dbItem != null)
                {
                    var originalItem = originalItems.Single(x => IdsAreEqual(getItemId(x), itemId));
                    if (onUpdateItem != null) await onUpdateItem(item, originalItem, dbItem);
                }
                else if (originalItems.All(x => !IdsAreEqual(getItemId(x), itemId)))
                {
                    if (onAddItem != null) await onAddItem(item);
                }
            }

            foreach (var item in dbItems)
            {
                var itemId = getItemId(item);
                var found = !Equals(itemId, default(TKey))
                    ? currentItems.SingleOrDefault(x => IdsAreEqual(getItemId(x), itemId))
                    : null;
                if (found == null && originalItems.Any(x => IdsAreEqual(getItemId(x), itemId)))
                {
                    if (onDeleteItem != null) await onDeleteItem(item);
                }
            }
        }

        public virtual async Task UpdateEntityAsync<T>(
            T currentItem,
            T originalItem = null,
            T dbItem = null,
            Func<EntityPropertyUpdatedEventArgs<T>, Task> onPropertyChanged = null,
            Func<EntityUpdatedEventArgs<T>, Task> onUpdateComplete = null,
            bool updateNestedEntitiesAndCollections = false,
            Func<T, object> getItemId = null)
            where T : class, IEntityBase
        {
            currentItem.NotNull(nameof(currentItem));
            dbItem.NotNull(nameof(dbItem));
            originalItem ??= dbItem;

            var modified = false;
            var entityType = currentItem.GetType();
            var entityName = GetFullObjectName(entityType);

            if (await UpdateEntityPropertiesAsync(
                currentItem, 
                null,
                currentItem, 
                originalItem, 
                dbItem,
                onPropertyChanged))
            {
                modified = true;
            }

            if (await UpdateEntitySetMethodsAsync(
                currentItem, 
                null, 
                currentItem, 
                originalItem, 
                dbItem,
                onPropertyChanged))
            {
                modified = true;
            }

            if (onUpdateComplete != null)
            {
                var eventArgs = new EntityUpdatedEventArgs<T>(currentItem);
                await onUpdateComplete(eventArgs);
            }
        }

        public async Task<bool> UpdateCompositeEntityAsync<T, TComposite>(
            T currentRootItem,
            string namePrefix,
            TComposite currentItem,
            TComposite originalItem,
            TComposite dbItem,
            Func<EntityPropertyUpdatedEventArgs<T>, Task> onPropertyChanged)
            where T : class, IEntityBase
            where TComposite : class, ICompositeEntity
        {
            currentItem.NotNull(nameof(currentItem));
            dbItem.NotNull(nameof(dbItem));
            originalItem ??= dbItem;

            var modified = false;
            var entityType = currentItem.GetType();
            if (await UpdateEntityPropertiesAsync(
                currentRootItem, 
                namePrefix, 
                currentItem, 
                originalItem,
                dbItem, 
                onPropertyChanged))
            {
                modified = true;
            }

            if (await UpdateEntitySetMethodsAsync<T>(
                currentRootItem, 
                namePrefix, 
                currentItem, 
                originalItem, 
                dbItem,
                onPropertyChanged))
            {
                modified = true;
            }

            return modified;
        }

        protected virtual object GetEntityId<T>(T entity) where T : class
        {
            return (entity as IEntity)?.Id ?? entity.GetPropertyValue("Id");
        }

        protected virtual async Task<bool> UpdateEntityPropertiesAsync<T>(
            T currentRootItem, 
            string namePrefix,
            object currentItem, 
            object originalItem, 
            object dbItem,
            Func<EntityPropertyUpdatedEventArgs<T>, Task> onPropertyChanged)
        where T : class, IEntityBase
        {
            var entityType = typeof(T);
            var modified = false;

            var properties = entityType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute<NotMappedAttribute>() == null &&
                            IsUpdatablePropertyType(p.PropertyType) &&
                            p.GetSetMethod() != null)
                .Select(p =>
                    new
                    {
                        Property = p,
                        UpdateStrategy = p.GetCustomAttribute<UpdateStrategyAttribute>(),
                        FullName = GetFullObjectName(p, namePrefix)
                    })
                .Where(p => p.UpdateStrategy.EnableUpdating)
                .ToList();

            foreach (var prop in properties)
            {
                var dbValue = prop.Property.TryGetValue(dbItem);
                var oldValue = prop.Property.TryGetValue(originalItem);
                var newValue = prop.Property.TryGetValue(currentItem);

                if (prop.Property.PropertyType.ImplementsInterface<ICompositeEntity>())
                {
                    var compositeModified = await UpdateCompositeEntityAsync(
                        currentRootItem, 
                        $"{prop.FullName}.",
                        (ICompositeEntity) newValue,
                        (ICompositeEntity) oldValue, 
                        (ICompositeEntity) dbValue,
                        onPropertyChanged);
                    if (compositeModified) modified = true;
                    continue;
                }

                if (Equals(oldValue, newValue) || Equals(dbValue, newValue)) continue;
                if (onPropertyChanged != null)
                {
                    var eventArgs = new EntityPropertyUpdatedEventArgs<T>(currentRootItem);
                    await onPropertyChanged(eventArgs);
                }
                prop.Property.SetValue(dbItem, newValue);
                modified = true;
            }

            return modified;
        }

        protected virtual async Task<bool> UpdateEntitySetMethodsAsync<T>(T currentRootItem, string namePrefix,
            object currentItem, object originalItem, object dbItem,
            Func<EntityPropertyUpdatedEventArgs<T>, Task> onPropertyChanged)
            where T : class, IEntityBase
        {
            var entityType = typeof(T);
            var modified = false;
            
            var setMethods = entityType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Select(x => new
                {
                    Method = x,
                    Attribute = x.GetCustomAttribute<UpdateStrategyAttribute>()
                })
                .Where(x => x.Attribute != null && !string.IsNullOrWhiteSpace(x.Attribute.Property))
                .ToList();

            foreach (var setter in setMethods)
            {
                var prop = entityType.GetProperty(setter.Attribute.Property);

                var dbValue = prop.TryGetValue(dbItem);
                var oldValue = prop.TryGetValue(originalItem);
                var newValue = prop.TryGetValue(currentItem);

                if (Equals(oldValue, newValue) || Equals(dbValue, newValue)) continue;

                if (onPropertyChanged != null)
                {
                    var propName = GetFullObjectName(prop, namePrefix);
                    var eventArgs = new EntityPropertyUpdatedEventArgs<T>(currentRootItem);
                    await onPropertyChanged(eventArgs);
                }
                setter.Method.Invoke(dbItem, new[] {newValue});
                modified = true;
            }

            return modified;
        }

        protected virtual string GetFullObjectName(object obj, string namePrefix = null)
        {
            switch (obj)
            {
                case Type _:
                {
                    var objType = (Type) obj;
                    return $"{namePrefix}{objType.Name}";
                }
                case PropertyInfo _:
                    var propInfo = (PropertyInfo) obj;
                    return $"{namePrefix}{propInfo.Name}";
                default:
                {
                    var objType = obj.GetType();
                    return $"{namePrefix}{objType.Name}";
                }
            }
        }

        protected virtual bool IsUpdatablePropertyType(Type propType)
        {
            return propType.IsSimpleType() || propType.ImplementsInterface<ICompositeEntity>();
        }
    }
}