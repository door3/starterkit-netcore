using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using D3SK.NetCore.Common.Entities;

namespace D3SK.NetCore.Common.Utilities
{
    public static class LookupHelper
    {
        private static readonly ConcurrentDictionary<Type, IList<object>> LookupsCache =
            new ConcurrentDictionary<Type, IList<object>>();

        public static T GetLookup<T>(int id, bool mustExist = true)
            where T : ILookupEntity<int>
        {
            return GetLookup<T, int>(id, mustExist);
        }

        public static T GetLookup<T, TKey>(TKey id, bool mustExist = true)
            where T : ILookupEntity<TKey>
            where TKey : IComparable
        {
            var items = GetAll<T, TKey>();
            return mustExist ? items.Single(x => x.Id.Equals(id)) : items.SingleOrDefault(x => x.Id.Equals(id));
        }

        public static IEnumerable<T> GetAll<T>()
            where T : ILookupEntity<int>
        {
            return GetAll<T, int>();
        }

        public static IEnumerable<T> GetAll<T, TKey>()
            where T : ILookupEntity<TKey>
            where TKey : IComparable
        {
            if (LookupsCache.TryGetValue(typeof(T), out var cache))
            {
                return cache.Cast<T>();
            }

            var items = GetAllStaticEntities<T, TKey>().ToList();
            LookupsCache.TryAdd(typeof(T), items.Cast<object>().ToList());
            return items;
        }

        public static IEnumerable<T> GetAllStaticEntities<T>()
            where T : IEntity<int>
        {
            return GetAllStaticEntities<T, int>();
        }

        public static IEnumerable<T> GetAllStaticEntities<T, TKey>()
            where T : IEntity<TKey>
            where TKey : IComparable
        {
            return typeof(T).GetTypeInfo()
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Select(x => x.GetValue(null))
                .OfType<T>();
        }
    }
}