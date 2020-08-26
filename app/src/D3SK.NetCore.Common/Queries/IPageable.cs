namespace D3SK.NetCore.Common.Queries
{
    public interface IPageable
    {
        int CurrentPage { get; set; }

        int PageSize { get; set; }
    }
}