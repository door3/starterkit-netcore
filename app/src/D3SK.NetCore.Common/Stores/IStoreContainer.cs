namespace D3SK.NetCore.Common.Stores
{
    public interface IStoreContainer
    {
    }
    
    public interface IStoreContainer<out TStore> : IStoreContainer where TStore : IStore
    {
        TStore Store { get; }
    }
}
