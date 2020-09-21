using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Queries;
using Xunit;

namespace D3SK.NetCore.Tests.Common.Queries
{
    public class TestEnumerableQueryParser
    {
        [Fact]
        public void ParseDynamicFilterTest()
        {
            TestDefaultIdEqualFilter();
            TestIdNotEqualFilter();
            TestStringContainsFilter();
            TestStringDoesNotContainFilter();
            TestDateTimeOffsetGreaterThanFilter();
            TestDecimalLessThanOrEqualToFilter();
            TestAndFilter();
            TestOrFilter();
            TestAndOrFilter();
            TestAndWithSubOrFilter();
            TestOrWithSubAndFilter();

            // test default id equal filter
            void TestDefaultIdEqualFilter()
            {
                var defaultIdEqualFilter = new QueryFilter(87);
                var (filter, parameters) = defaultIdEqualFilter.ParseDynamicFilter();
                Assert.Equal("(Id == @0)", filter);
                Assert.Collection(parameters, i => Assert.Equal(87, i));
            }

            // test id not equal filter
            void TestIdNotEqualFilter()
            {
                var idNotEqualFilter = new QueryFilter("id", 44, QueryFilterOperators.NotEqual);
                var (filter, parameters) = idNotEqualFilter.ParseDynamicFilter();
                Assert.Equal("(Id != @0)", filter);
                Assert.Collection(parameters, i => Assert.Equal(44, i));
            }

            // test string contains
            void TestStringContainsFilter()
            {
                var stringContainsFilter = new QueryFilter("myString", "test", QueryFilterOperators.Contains);
                var (filter, parameters) = stringContainsFilter.ParseDynamicFilter();
                Assert.Equal("(MyString.Contains(@0))", filter);
                Assert.Collection(parameters, i => Assert.Equal("test", i));
            }

            // test string not contains
            void TestStringDoesNotContainFilter()
            {
                var stringDoesNotContainFilter = new QueryFilter("SomeString", "don't want this", QueryFilterOperators.DoesNotContain);
                var (filter, parameters) = stringDoesNotContainFilter.ParseDynamicFilter();
                Assert.Equal("(!SomeString.Contains(@0))", filter);
                Assert.Collection(parameters, i => Assert.Equal("don't want this", i));
            }

            // test dateTimeOffset greater than
            void TestDateTimeOffsetGreaterThanFilter()
            {
                var dateTimeOffsetString = "2020-09-18T03:52:00Z";
                var dateTimeOffsetGreaterThanFilter = new QueryFilter("MyDate", DateTimeOffset.Parse(dateTimeOffsetString), QueryFilterOperators.GreaterThan);
                var (filter, parameters) = dateTimeOffsetGreaterThanFilter.ParseDynamicFilter();
                Assert.Equal("(MyDate > @0)", filter);
                Assert.Collection(parameters, i => Assert.Equal(DateTimeOffset.Parse(dateTimeOffsetString), i));
            }

            // test decimal less than or equal to
            void TestDecimalLessThanOrEqualToFilter()
            {
                var decimalLessThanOrEqualToFilter = new QueryFilter("MyDecimal", 193.6604m, QueryFilterOperators.LessThanOrEqual);
                var (filter, parameters) = decimalLessThanOrEqualToFilter.ParseDynamicFilter();
                Assert.Equal("(MyDecimal <= @0)", filter);
                Assert.Collection(parameters, i => Assert.Equal(193.6604m, i));
            }

            // test andFilter
            void TestAndFilter()
            {
                var andFilter = new QueryFilter(64);
                andFilter.AndFilters.Add(new QueryFilter(90));
                andFilter.AndFilters.Add(new QueryFilter(120));
                var (filter, parameters) = andFilter.ParseDynamicFilter();
                Assert.Equal("(Id == @0 and (Id == @1) and (Id == @2))", filter);
                Assert.Collection(parameters,
                    i => Assert.Equal(64, i),
                    i => Assert.Equal(90, i),
                    i => Assert.Equal(120, i));
            }

            // test orFilter
            void TestOrFilter()
            {
                var orFilter = new QueryFilter(19);
                orFilter.OrFilters.Add(new QueryFilter(1028));
                orFilter.OrFilters.Add(new QueryFilter(2));
                var (filter, parameters) = orFilter.ParseDynamicFilter();
                Assert.Equal("(Id == @0 or (Id == @1) or (Id == @2))", filter);
                Assert.Collection(parameters,
                    i => Assert.Equal(19, i),
                    i => Assert.Equal(1028, i),
                    i => Assert.Equal(2, i));
            }

            // test andFilter and orFilter together
            void TestAndOrFilter()
            {
                var andOrFilter = new QueryFilter(201);
                andOrFilter.AndFilters.Add(new QueryFilter(455));
                andOrFilter.OrFilters.Add(new QueryFilter(80));
                andOrFilter.AndFilters.Add(new QueryFilter(456));
                andOrFilter.OrFilters.Add(new QueryFilter(81));
                var (filter, parameters) = andOrFilter.ParseDynamicFilter();
                Assert.Equal("(Id == @0 and (Id == @1) and (Id == @2) or (Id == @3) or (Id == @4))", filter);
                Assert.Collection(parameters,
                    i => Assert.Equal(201, i),
                    i => Assert.Equal(455, i),
                    i => Assert.Equal(456, i),
                    i => Assert.Equal(80, i),
                    i => Assert.Equal(81, i));
            }

            // test andFilter with sub orFilters
            void TestAndWithSubOrFilter()
            {
                var andFilter = new QueryFilter(9001);
                var orFilter = new QueryFilter(717);
                orFilter.OrFilters.Add(new QueryFilter(56));
                andFilter.AndFilters.Add(orFilter);
                var (filter, parameters) = andFilter.ParseDynamicFilter();
                Assert.Equal("(Id == @0 and (Id == @1 or (Id == @2)))", filter);
                Assert.Collection(parameters,
                    i => Assert.Equal(9001, i),
                    i => Assert.Equal(717, i),
                    i => Assert.Equal(56, i));
            }

            // test orFilter with sub andFilters
            void TestOrWithSubAndFilter()
            {
                var orFilter = new QueryFilter(8063);
                var andFilter = new QueryFilter(973);
                andFilter.AndFilters.Add(new QueryFilter(170));
                orFilter.OrFilters.Add(andFilter);
                var (filter, parameters) = orFilter.ParseDynamicFilter();
                Assert.Equal("(Id == @0 or (Id == @1 and (Id == @2)))", filter);
                Assert.Collection(parameters,
                    i => Assert.Equal(8063, i),
                    i => Assert.Equal(973, i),
                    i => Assert.Equal(170, i));
            }
        }
    }
}
