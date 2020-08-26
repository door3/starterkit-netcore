using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3SK.NetCore.Common.Extensions
{
    public static class ListExtensions
    {
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

            var newList = new List<TReturnType>();
            foreach (var item in list)
            {
                newList.Add(action(item));
            }

            return newList;
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

        public static T[,] ToMultidimensionalArray<T>(this IList<T[]> arrays)
        {
            var minorLength = arrays[0].Length;
            T[,] ret = new T[arrays.Count, minorLength];
            for (int i = 0; i < arrays.Count; i++)
            {
                var array = arrays[i];
                if (array.Length != minorLength)
                {
                    throw new ArgumentException
                        ("All arrays must be the same length");
                }
                for (int j = 0; j < minorLength; j++)
                {
                    ret[i, j] = array[j];
                }
            }
            return ret;
        }

        public static T[,] ToMultidimensionalArray<T>(this IEnumerable<T[]> arrays)
        {
            return arrays.ToList().ToMultidimensionalArray();
        }

        public static IList<string> Strip(this IList<string> source)
        {
            var list = new List<string>(source.Count);
            list.AddRange(source.Select(x => x.Strip()));
            return list;
        }

        public static T TryGet<T>(this IList<T> source, int index, T defaultValue = default(T))
        {
            if (source == null) return defaultValue;

            return source.Count > index ? source[index] : defaultValue;
        }

        public static bool IsOnlyOne<T>(this IEnumerable<T> source, Func<T, bool> predicate = null)
        {
            source = predicate == null ? source : source.Where(predicate);
            return source.Count() <= 1;
        }

        public static bool IsExactlyOne<T>(this IEnumerable<T> source, Func<T, bool> predicate = null)
        {
            source = predicate == null ? source : source.Where(predicate);
            return source.Count() == 1;
        }

        public static bool IsAtLeast<T>(this IEnumerable<T> source, int atLeastNum, Func<T, bool> predicate = null)
        {
            if (source == null) return atLeastNum > 0 ? false : true;
            source = predicate == null ? source : source.Where(predicate);
            return source.Count() >= atLeastNum;
        }

        public static IEnumerable<T> AtLeast<T>(this IEnumerable<T> source, int atLeastNum,
            string argumentName = null)
        {
            source.NotNull(nameof(argumentName));
            if (!source.IsAtLeast(atLeastNum))
            {
                throw new ArgumentException($"List must contain at least {atLeastNum} items.", argumentName);
            }

            return source;
        }

        public static IEnumerable<T> NoMoreThan<T>(this IEnumerable<T> source, int noMoreThan, string argumentName = null)
        {
            source.NotNull(nameof(argumentName));
            if (!source.IsNoMoreThan(noMoreThan))
            {
                throw new ArgumentException($"List must contain no more than {noMoreThan} items.");
            }
            return source;
        }

        public static bool IsNoMoreThan<T>(this IEnumerable<T> source, int noMoreThan, Func<T, bool> predicate = null)
        {
            if (source == null) return noMoreThan == 0;
            source = predicate == null ? source : source.Where(predicate);
            return source.Count() <= noMoreThan;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        public static IEnumerable<T> DistinctBy<T, TDistinctKey>(this IEnumerable<T> source,
            Func<T, TDistinctKey> keySelector)
        {
            return source.GroupBy(keySelector).Select(x => x.First());
        }

        public static IEnumerable<T> Slim<T>(this IEnumerable<T> source)
        {
            return source.Where(x => x != null);
        }

        public static void TryAddRange<T>(this IList<T> source, IEnumerable<T> collection)
        {
            if (source == null || collection == null || !collection.Any()) return;

            source.AddRange(collection);
        }

        public static IOrderedEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source?.OrderBy(x => RandomHelper.Generator.Next());
        }
    }
}
