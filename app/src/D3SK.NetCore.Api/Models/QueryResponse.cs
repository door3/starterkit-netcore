using System.Collections.Generic;
using System.Collections.Immutable;

namespace D3SK.NetCore.Api.Models
{
    public class SearchQueryResponse<T>
    {
        public IReadOnlyList<T> Items { get; }

        public int ItemCount => Items?.Count ?? 0;

        public int TotalItemCount { get; }

        public SearchQueryResponse(IEnumerable<T> items, int totalItemCount)
        {
            Items = items.ToImmutableList();
            TotalItemCount = totalItemCount;
        }
    }
}