using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3SK.NetCore.Common.Entities;

namespace D3SK.NetCore.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static void RemoveEntity<T>(this ICollection<T> list, T item) where T : IEntity<int>
        {
            RemoveEntity<T, int>(list, item);
        }

        public static void RemoveEntity<T, TKey>(this ICollection<T> list, T item) where T : IEntity<TKey>
        {
            list.NotNull(nameof(list));
            var listItem = list.Single(x => x.Id.Equals(item.Id));
            list.Remove(listItem);
        }

        public static bool RemoveIfExists<T>(this ICollection<T> list, Func<T, bool> predicate, bool isSingle = true)
        {
            list.NotNull(nameof(list));
            predicate.NotNull(nameof(predicate));

            var items = list.Where(predicate).ToList();

            if (isSingle)
            {
                var item = items.SingleOrDefault();
                if (item != null)
                {
                    list.Remove(item);
                }
            }
            else
            {
                foreach (var item in items)
                {
                    list.Remove(item);
                }
            }

            return items.Any();
        }

        public static bool AddIfMissing<T>(this ICollection<T> list, T item, Func<T, bool> predicate)
        {
            list.NotNull(nameof(list));
            item.NotNull(nameof(item));
            predicate.NotNull(nameof(predicate));

            var items = list.Where(predicate).ToList();

            if (items.Any()) return false;

            list.Add(item);
            return true;
        }
    }
}
