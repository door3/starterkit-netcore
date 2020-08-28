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

        Task DeleteAsync(int id);

        Task UpdateAsync(T currentItem, T originalItem = null);
    }
}
