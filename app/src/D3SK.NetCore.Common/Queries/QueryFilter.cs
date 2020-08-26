using System;
using System.Collections.Generic;
using System.Linq;

namespace D3SK.NetCore.Common.Queries
{
    public class QueryFilterOperators
    {
        public const string Equal = "eq";

        public const string NotEqual = "neq";

        public const string LessThan = "lt";

        public const string LessThanOrEqual = "lte";

        public const string GreaterThan = "gt";

        public const string GreaterThanOrEqual = "gte";

        public const string Contains = "contains";

        public const string DoesNotContain = "not contains";

        public const string Raw = "raw";
    }

    public class QueryFilterPropertyTypes
    {
        public const string String = "string";

        public const string Integer = "int";

        public const string Double = "double";

        public const string Date = "date";

        public const string List = "list";

    }

    public class QueryFilter : ICloneable
    {
        public const string DefaultProperty = "Id";

        public QueryFilter()
        {
        }

        public QueryFilter(object value)
        {
            Value = value;
        }

        public QueryFilter(string property, object value, string @operator = QueryFilterOperators.Equal,
            string propertyType = null)
        {
            Property = property;
            Value = value;
            Operator = @operator;
        }

        public string Property { get; set; } = DefaultProperty;

        public string PropertyType { get; set; }

        public string Operator { get; set; } = QueryFilterOperators.Equal;

        public object Value { get; set; }

        public IList<QueryFilter> AndFilters { get; set; } = new List<QueryFilter>();

        public IList<QueryFilter> OrFilters { get; set; } = new List<QueryFilter>();

        public object Clone()
        {
            var clone = (QueryFilter) MemberwiseClone();
            clone.AndFilters = new List<QueryFilter>(AndFilters.Select(x => (QueryFilter) x.Clone()));
            clone.OrFilters = new List<QueryFilter>(OrFilters.Select(x => (QueryFilter) x.Clone()));

            return clone;
        }
    }
}