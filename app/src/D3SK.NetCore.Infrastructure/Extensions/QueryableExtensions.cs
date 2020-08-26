using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Common.Stores;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace D3SK.NetCore.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<IQueryable<T>> WhereAsync<T>(this IQueryable<T> source, string filter)
        {
            source.NotNull(nameof(source));

            var options = ScriptOptions.Default.AddReferences(typeof(T).Assembly);
            var predicate = await CSharpScript.EvaluateAsync<Func<T, bool>>(filter, options);
            Expression<Func<T, bool>> expression = f => predicate(f);
            return source.Where(expression);
        }

        public static async Task<IQueryable<T>> FilterAsync<T>(this IQueryable<T> source, params QueryFilter[] filters)
        {
            source.NotNull(nameof(source));

            if (filters == null)
            {
                return source;
            }

            await filters.ForEachAsync(async f =>
            {
                var filter = EnumerableQueryParser.ParseFilter(f);
                source = await source.WhereAsync(filter);
            });

            return source;
        }

        public static async Task<IQueryable<T>> FilterAsync<T>(this IQueryable<T> source, IList<QueryFilter> filters)
        {
            return await source.FilterAsync(filters?.ToArray());
        }

        public static async Task<IQueryable<T>> FilterAsync<T>(this IQueryable<T> source, IFilterable filterInfo)
        {
            return await source.FilterAsync(filterInfo?.Filters);
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> source, params QueryFilter[] filters)
        {
            source.NotNull(nameof(source));

            if (filters == null)
            {
                return source;
            }

            filters.ForEach(f =>
            {
                var (filter, parameters) = EnumerableQueryParser.ParseDynamicFilter(f);
                source = source.Where(filter, parameters.ToArray());
            });

            return source;
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> source, IList<QueryFilter> filters)
        {
            return source.Filter(filters?.ToArray());
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> source, IFilterable filterInfo)
        {
            return source.Filter(filterInfo?.Filters);
        }

        public static bool IsIdFilter(this IFilterable filterInfo, params string[] primaryKeys)
        {
            if (filterInfo?.Filters == null) return false;

            foreach (var key in primaryKeys)
            {
                if (filterInfo.Filters.Count(x => x.Property.Equals(key, StringComparison.OrdinalIgnoreCase)) != 1)
                    return false;
            }

            return filterInfo.Filters.Count == primaryKeys.Length;
        }

        public static bool IsIdFilter(this IFilterable filterInfo)
        {
            return filterInfo.IsIdFilter("Id");
        }

        public static void AddFilterList<T>(this IFilterable filterInfo, IList<T> items,
            string property = QueryFilter.DefaultProperty, string @operator = QueryFilterOperators.Equal,
            string propertyType = null, bool isOrQuery = true)
        {
            if (!items.Any())
                return;

            var filter = GetFilter(items.First());
            filterInfo.Filters.Add(filter);
            var filterList = isOrQuery ? filter.OrFilters : filter.AndFilters;

            items.Skip(1).ForEach(x => { filterList.Add(GetFilter(x)); });

            QueryFilter GetFilter(T value)
            {
                return new QueryFilter(property, value, @operator, propertyType);
            }
        }

        public static string GetIncludes(this IStoreQuery query)
        {
            return query?.Includes ?? (query.IsIdFilter() ? StoreQueryIncludes.Full : null);
        }

        public static bool HasInclude(this IStoreQuery query, string include)
        {
            var includesList = query?.Includes?.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (includesList == null || includesList.Any())
            {
                return false;
            }

            return includesList.Contains(include, StringComparer.OrdinalIgnoreCase);
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> source, IPageable pagingInfo)
        {
            source.NotNull(nameof(source));
            return pagingInfo == null || pagingInfo.PageSize <= 0 
                ? source 
                : source.Skip(pagingInfo.CurrentPage * pagingInfo.PageSize).Take(pagingInfo.PageSize);
        }

        public static void PageWhile<T>(this IEnumerable<T> source, Func<IEnumerable<T>, bool> pageAction, int pageSize = 100)
        {
            var count = source.Count();
            for (int i = 0; i < count; i += pageSize)
            {
                var list = source.Skip(i).Take(pageSize);
                if (!pageAction(list))
                {
                    break;
                }
            }
        }

        public static IQueryable Project<T>(this IQueryable<T> source, IProjection projection)
        {
            source.NotNull(nameof(source));

            if (projection == null || projection.SelectProperties?.FirstOrDefault() == null)
            {
                return source;
            }

            return source.Select($"new ({projection.SelectProperties.Aggregate((x, y) => x + "," + y)})");
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> source, ISortable sortInfo, string defaultSortField = "Id")
        {
            source.NotNull(nameof(source));

            if (string.IsNullOrWhiteSpace(sortInfo?.SortField))
            {
                return source.OrderBy(defaultSortField);
            }

            var fields = sortInfo?.SortField?.Split(",", StringSplitOptions.RemoveEmptyEntries);
            var directions = sortInfo.SortDirection?.Split(",", StringSplitOptions.RemoveEmptyEntries);

            var currentDirection = SortDirections.Ascending;
            IOrderedQueryable<T> ordered = null;
            for (var i = 0; i < fields.Length; i++)
            {
                var field = fields[i].ToPropertyCase();
                var dir = directions?.Skip(i).FirstOrDefault();
                currentDirection = dir ?? currentDirection;
                if (i == 0) ordered = source.OrderBy($"{field} {currentDirection}");
                else ordered = ordered.ThenBy($"{field} {currentDirection}");
            }

            return ordered;
        }
    }
}