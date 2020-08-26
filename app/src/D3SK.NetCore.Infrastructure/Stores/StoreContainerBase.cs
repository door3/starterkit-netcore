using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Stores;

namespace D3SK.NetCore.Infrastructure.Stores
{
    public abstract class StoreContainerBase<TStore> : IStoreContainer<TStore> where TStore : IStore
    {
        public TStore Store { get; }

        protected StoreContainerBase(TStore store)
        {
            Store = store.NotNull(nameof(store));
        }
    }
}
