using System.Collections.Generic;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Stores;

namespace D3SK.NetCore.Api.Models
{
    public abstract class QueryRequestBase<TQuery> where TQuery : IQueryFeature
    {
        public abstract void SetQuery(TQuery query);
    }

    public abstract class SearchQueryRequestBase<TQuery> : QueryRequestBase<TQuery>, IPageable, ISortable
        where TQuery : IStoreQueryFeature
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public string SortDirection { get; set; }

        public string SortField { get; set; }

        public bool TrackEntities { get; set; }
        
        public string Includes { get; set; }

        public override void SetQuery(TQuery query)
        {
            query.NotNull(nameof(query));

            query.CurrentPage = CurrentPage;
            query.PageSize = PageSize < 0 ? 0 : PageSize.Default(query.PageSize);
            query.SortDirection = SortDirection;
            query.SortField = SortField;
            query.TrackEntities = TrackEntities.Default(query.TrackEntities);
            query.Includes = Includes;
        }

        public void SetQueryAndCount(TQuery query, IFilterable countQuery)
        {
            SetQuery(query);
            countQuery.Filters = query.Filters;
        }
    }

    public class FromQuerySearchQueryRequest<TQuery> : SearchQueryRequestBase<TQuery> where TQuery : IStoreQueryFeature
    {
        public string Filters { get; set; }

        public override void SetQuery(TQuery query)
        {
            base.SetQuery(query);
            if (!string.IsNullOrWhiteSpace(Filters))
            {
                query.Filters = JsonHelper.Deserialize<IList<QueryFilter>>(Filters);
            }
        }
    }

    public class FromQuerySearchQueryRequest : FromQuerySearchQueryRequest<IStoreQueryFeature>
    {
    }

    public class FromQueryProjectionSearchQueryRequest : FromQuerySearchQueryRequest<IProjectionStoreQueryFeature>,
        IAllowDistinctQuery
    {
        public string SelectProperties { get; set; }

        public bool Distinct { get; set; }

        public override void SetQuery(IProjectionStoreQueryFeature query)
        {
            base.SetQuery(query);
            if (!string.IsNullOrWhiteSpace(SelectProperties))
            {
                query.SelectProperties = JsonHelper.Deserialize<IList<string>>(SelectProperties);
            }

            query.Distinct = Distinct;
        }
    }

    public class FromBodySearchQueryRequest<TQuery> : SearchQueryRequestBase<TQuery>, IStoreQueryFeature
        where TQuery : IStoreQueryFeature
    {
        public IList<QueryFilter> Filters { get; set; } = new List<QueryFilter>();

        public override void SetQuery(TQuery query)
        {
            base.SetQuery(query);
            query.Filters = Filters;
        }
    }

    public class FromBodySearchQueryRequest : FromBodySearchQueryRequest<IStoreQueryFeature>
    {
    }

    public class FromBodyProjectionSearchQueryRequest : FromBodySearchQueryRequest<IProjectionStoreQueryFeature>,
        IAllowDistinctQuery
    {
        public IList<string> SelectProperties { get; set; } = new List<string>();

        public bool Distinct { get; set; }

        public override void SetQuery(IProjectionStoreQueryFeature query)
        {
            base.SetQuery(query);
            query.SelectProperties = SelectProperties;
            query.Distinct = Distinct;
        }
    }
}