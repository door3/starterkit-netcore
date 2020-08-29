using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;

namespace D3SK.NetCore.Common.Stores
{
    public interface ICommandContainer : IStoreContainer
    {
    }

    public interface ICommandContainer<out TStore> : IStoreContainer<TStore>, ICommandContainer where TStore : ICommandStore
    {
    }

    public interface ICommandContainer<in T, out TStore> : ICommandContainer<TStore>
        where T : class, IEntityBase
        where TStore : ICommandStore
    {
        Task AddAsync(T item);
        
        Task UpdateAsync(T currentItem, T originalItem = null);
    }

    public interface ICommandContainer<T, in TKey, out TStore> : ICommandContainer<T, TStore>
        where T : class, IEntity<TKey>
        where TStore : ICommandStore
    {
        Task DeleteAsync(TKey id);

        Task<T> FindAsync(TKey id);
    }
}
