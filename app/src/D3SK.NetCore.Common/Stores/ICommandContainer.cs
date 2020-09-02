using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;

namespace D3SK.NetCore.Common.Stores
{
    public interface ICommandContainerBase : IStoreContainer
    {
    }

    public interface ICommandContainerBase<out TStore> : IStoreContainer<TStore>, ICommandContainerBase where TStore : ICommandStore
    {
    }

    public interface ICommandContainerBase<in T, out TStore> : ICommandContainerBase<TStore>
        where T : class, IEntityBase
        where TStore : ICommandStore
    {
        Task AddAsync(T item);

        Task UpdateAsync(T currentItem, T originalItem = null);
    }

    public interface ICommandContainer<T, out TStore> : ICommandContainer<T, int, TStore>
        where T : class, IEntity<int>
        where TStore : ICommandStore
    {
    }

    public interface ICommandContainer<T, in TKey, out TStore> : ICommandContainerBase<T, TStore>
        where T : class, IEntity<TKey>
        where TStore : ICommandStore
    {
        Task DeleteAsync(TKey id);

        Task<T> FindAsync(TKey id);
    }
}
