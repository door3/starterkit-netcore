using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;

namespace D3SK.NetCore.Common.Stores
{
    public interface IConcurrencyContainer<in T, out TStore> : ICommandContainerBase<TStore>
        where T : class, IEntityBase
        where TStore : ICommandStore
    {
        Task UpdateRowVersionAsync(T currentItem, T dbItem = null);
    }
}
