namespace D3SK.NetCore.Common.Queries
{
    public class QueryOptions
    {
        public int DefaultPageSize { get; set; } = 100;

        public bool TrackEntities { get; set; } = false;
    }
}
