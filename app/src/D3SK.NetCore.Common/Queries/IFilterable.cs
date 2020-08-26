using System.Collections.Generic;

namespace D3SK.NetCore.Common.Queries
{
    public interface IFilterable
    {
        IList<QueryFilter> Filters { get; set; }
    }

    public class FilterQuery : IFilterable
    {
        public IList<QueryFilter> Filters { get; set; } = new List<QueryFilter>();

        public FilterQuery()
        {
        }

        public FilterQuery(params QueryFilter[] filters)
        {
            if (filters != null)
            {
                Filters = filters;
            }
        }
    }
}