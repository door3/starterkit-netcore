using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Common.Queries
{
    public static class EnumerableQueryParser
    {
        public static (string, IList<object>) ParseDynamicFilter(this QueryFilter filter)
        {
            var parameters = new List<object>();
            var filterString = ParseFilterCore(filter, 0, true, ref parameters);

            return (filterString, parameters);
        }

        public static string ParseFilter(this QueryFilter filter)
        {
            var parameters = new List<object>();
            return ParseFilterCore(filter, 0, false, ref parameters);
        }

        private static string ParseFilterCore(QueryFilter filter, int level, bool isDynamicFilter, ref List<object> parameters)
        {
            filter.NotNull(nameof(filter));

            var andValue = isDynamicFilter ? "and" : "&&";
            var orValue = isDynamicFilter ? "or" : "||";
            var filterProperty = filter.Property.ToPropertyCase();
            var (filterOperator, isOperatorFunction, isNegated) = ParseFilterOperator(filter.Operator, filter.PropertyType);
            var filterValue = isDynamicFilter 
                ? $"@{parameters.Count}"
                : ParseFilterValue(filter.Value, filter.PropertyType);

            var parameterValue = ConvertParameterValue(filter.Value);
            parameters.Add(parameterValue);

            var isList = filter.PropertyType == QueryFilterPropertyTypes.List;

            string AddFilters(IList<QueryFilter> filters, string op,  ref List<object> localParameters)
            {
                var sb = new StringBuilder();
                foreach (var f in filters)
                {
                    if (sb.Length > 0) sb.Append($" {op} ");

                    var subLevel = isList ? 0 : level + 1;
                    sb.Append(ParseFilterCore(f, level + 1, isDynamicFilter, ref localParameters));
                }

                return sb.ToString();
            }

            var andFilters = AddFilters(filter.AndFilters, andValue, ref parameters);
            var orFilters = AddFilters(filter.OrFilters, orValue, ref parameters);

            var predicateVariable = !isDynamicFilter && level == 0 ? "x => " : string.Empty;
            var objectVariable = !isDynamicFilter ? "x." : string.Empty;

            if (isList) filterValue = andFilters;

            var filterExpression = (string) null;

            if (filterOperator == QueryFilterOperators.Raw)
            {
                filterExpression = filterProperty.Replace("@@", filterValue);
            }
            else
            {
                filterExpression = !isOperatorFunction
                    ? $"{predicateVariable}{(isNegated ? "!(" : string.Empty)}{objectVariable}{filterProperty} {filterOperator} {filterValue}{(isNegated ? ")" : string.Empty)}"
                    : $"{predicateVariable}{(isNegated ? "!" : string.Empty)}{objectVariable}{filterProperty}.{filterOperator}({filterValue})";
            }

            var sbExpression = new StringBuilder(filterExpression);

            void AddExpressionFilters(string filters, string op)
            {
                if (filters.Length > 0) sbExpression.Append($" {op} {filters}");
            }

            if (!isList) AddExpressionFilters(andFilters, andValue);
            AddExpressionFilters(orFilters, orValue);

            return $"({sbExpression.ToString()})";
        }

        private static (string, bool, bool) ParseFilterOperator(string filterOperator, string propertyType)
        {
            switch (filterOperator)
            {
                case QueryFilterOperators.Equal:
                    return ("==", false, false);
                case QueryFilterOperators.NotEqual:
                    return ("!=", false, false);
                case QueryFilterOperators.LessThan:
                    return ("<", false, false);
                case QueryFilterOperators.LessThanOrEqual:
                    return ("<=", false, false);
                case QueryFilterOperators.GreaterThan:
                    return (">", false, false);
                case QueryFilterOperators.GreaterThanOrEqual:
                    return (">=", false, false);
                case QueryFilterOperators.Contains:
                    return (GetContainsMethodName(), true, false);
                case QueryFilterOperators.DoesNotContain:
                    return (GetContainsMethodName(), true, true);
                default:
                    return (filterOperator, false, false);
            }

            string GetContainsMethodName()
            {
                return propertyType == QueryFilterPropertyTypes.List ? "Any" : "Contains";
            }
        }

        private static string ParseFilterValue(object filterValue, string filterPropertyType = null)
        {
            if (filterValue == null) return "null";

            return SwitchType(string.IsNullOrWhiteSpace(filterPropertyType)
                ? filterValue.GetType().Name.ToLower()
                : filterPropertyType.ToLower());

            string SwitchType(string type)
            {
                switch (type)
                {
                    case QueryFilterPropertyTypes.String:
                        return filterValue.ToString().QuoteEncode();
                    default:
                        return filterValue.ToString();
                }
            }
        }

        private static object ConvertParameterValue(object filterValue)
        {
            return filterValue switch
            {
                double _ => Convert.ToDecimal(filterValue),
                DateTime date => new DateTimeOffset(date),
                _ => filterValue
            };
        }
    }
}