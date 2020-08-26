using System;
using D3SK.NetCore.Common.Queries;

namespace D3SK.NetCore.Common.Extensions
{
    public static class QueryFilterExtensions
    {
        public static QueryFilter Or(this QueryFilter baseFilter, QueryFilter orFilter)
        {
            baseFilter.OrFilters.Add(orFilter);

            return baseFilter;
        }

        public static QueryFilter Or(this QueryFilter baseFilter, string property, object value,
            string @operator = QueryFilterOperators.Equal, string propertyType = null)
        {
            return baseFilter.Or(new QueryFilter(property, value, @operator, propertyType));
        }

        public static QueryFilter And(this QueryFilter baseFilter, QueryFilter andFilter)
        {
            baseFilter.AndFilters.Add(andFilter);

            return baseFilter;
        }

        public static QueryFilter And(this QueryFilter baseFilter, string property, object value,
            string @operator = QueryFilterOperators.Equal, string propertyType = null)
        {
            return baseFilter.And(new QueryFilter(property, value, @operator, propertyType));
        }
    }
}
