using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Utilities;

namespace D3SK.NetCore.Common.Extensions
{
    public static class ListExtensions
    {
        public static IList<T> NotEmpty<T>(this IList<T> source, string argumentName = null)
        {
            if (source == null || source.Count == 0) throw new ArgumentNullException(argumentName);
            return source;
        }

        public static void ForEach<T>(this IList<T> list, Action<T> action)
        {
            list.NotNull(nameof(list));
            action.NotNull(nameof(action));

            foreach (var item in list) action(item);
        }

        public static IList<T> ForEachSelect<T>(this IList<T> list, Action<T> action)
        {
            list.NotNull(nameof(list));
            action.NotNull(nameof(action));

            foreach (var item in list)
            {
                action(item);
            }

            return list;
        }

        public static IList<TReturnType> ForEachSelect<T, TReturnType>(this IList<T> list, Func<T, TReturnType> action)
        {
            list.NotNull(nameof(list));
            action.NotNull(nameof(action));

            return list.Select(action).ToList();
        }

        public static async Task ForEachAsync<T>(this IList<T> list, Func<T, Task> action)
        {
            list.NotNull(nameof(list));
            action.NotNull(nameof(action));

            foreach (var item in list) await action(item);
        }

        public static async Task<IList<TReturnType>> ForEachSelectAsync<T, TReturnType>(this IList<T> list, Func<T, Task<TReturnType>> action)
        {
            list.NotNull(nameof(list));
            action.NotNull(nameof(action));

            var newList = new List<TReturnType>();
            foreach (var item in list)
            {
                newList.Add(await action(item));
            }

            return newList;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            list.NotNull(nameof(list));
            action.NotNull(nameof(action));

            foreach (var item in list.ToList())
            {
                action(item);
            }
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            list.NotNull(nameof(list));
            items.NotNull(nameof(items));

            if (list is List<T> localList)
            {
                localList.AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    list.Add(item);
                }
            }
        }

        public static void AddIfMissing<T>(this IList<T> list, T item)
        {
            list.NotNull(nameof(list));
            item.NotNull(nameof(item));

            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }

        public static void AddIfNotNull<T>(this IList<T> list, T item)
        {
            if (list != null && item != null) list.Add(item);
        }

        public static T PickRandom<T>(this IList<T> list, T defaultValue = default(T))
        {
            list.NotNull(nameof(list));

            if (!list.Any()) return defaultValue;

            return list[RandomHelper.Generator.Next(list.Count)];
        }

        public static T[,] ToMultidimensionalArray<T>(this IList<T[]> arrays)
        {
            var minorLength = arrays[0].Length;
            var ret = new T[arrays.Count, minorLength];
            for (var i = 0; i < arrays.Count; i++)
            {
                var array = arrays[i];
                if (array.Length != minorLength)
                {
                    throw new ArgumentException
                        ("All arrays must be the same length");
                }
                for (var j = 0; j < minorLength; j++)
                {
                    ret[i, j] = array[j];
                }
            }
            return ret;
        }

        public static IList<string> Strip(this IList<string> source)
        {
            var list = new List<string>(source.Count);
            list.AddRange(source.Select(x => x.Strip()));
            return list;
        }

        public static T TryGet<T>(this IList<T> source, int index, T defaultValue = default)
        {
            if (source == null) return defaultValue;

            return source.Count > index ? source[index] : defaultValue;
        }
    }
}