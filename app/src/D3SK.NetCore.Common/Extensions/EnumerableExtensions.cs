using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using D3SK.NetCore.Common.Utilities;

namespace D3SK.NetCore.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static T[,] ToMultidimensionalArray<T>(this IEnumerable<T[]> arrays)
        {
            return arrays.ToList().ToMultidimensionalArray();
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
            if (source == null) return atLeastNum <= 0;
            source = predicate == null ? source : source.Where(predicate);
            return source.Count() >= atLeastNum;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
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

        public static bool IsNoMoreThan<T>(this IEnumerable<T> source, int noMoreThan, Func<T, bool> predicate = null)
        {
            if (source == null) return noMoreThan == 0;
            source = predicate == null ? source : source.Where(predicate);
            return source.Count() <= noMoreThan;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> NoMoreThan<T>(this IEnumerable<T> source, int noMoreThan, string argumentName = null)
        {
            source.NotNull(nameof(argumentName));
            if (!source.IsNoMoreThan(noMoreThan))
            {
                throw new ArgumentException($"List must contain no more than {noMoreThan} items.");
            }
            return source;
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

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
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
