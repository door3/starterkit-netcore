using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Domain.Stores;

namespace D3SK.NetCore.Domain.Features
{
    public interface IEntityMultipleUpdateCommand<T> : IStoreCommandFeature
        where T : class, IEntityBase
    {
        Item<T>[] Items { get; set; }
    }

    public interface IEntityMultipleUpdateCommand<TDomain, T> : IAsyncCommandFeature<TDomain>, IEntityMultipleUpdateCommand<T>
        where TDomain : IDomain
        where T : class, IEntityBase
    {
    }

    public class Item<T>
        where T : class, IEntityBase
    {
        public T OriginalItem { get; set; }

        public T CurrentItem { get; set; }
    }
}
