using D3SK.NetCore.Common.Queries;

namespace D3SK.NetCore.Common.Stores
{
    public class StoreQueryIncludes
    {
        public const string None = "none";

        public const string Full = "full";

        public const string Slim = "slim";
    }

    public interface IStoreQuery : IPageable, ISortable, IFilterable
    {
        bool TrackEntities { get; set; }

        string Includes { get; set; }
    }

    public interface IProjectionStoreQuery : IStoreQuery, IProjection, IAllowDistinctQuery
    {
    }
}
