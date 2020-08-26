namespace D3SK.NetCore.Common.Queries
{
    public class SortDirections
    {
        public const string Ascending = "asc";

        public const string Descending = "desc";
    }

    public interface ISortable
    {
        string SortDirection { get; set; }

        string SortField { get; set; }
    }
}