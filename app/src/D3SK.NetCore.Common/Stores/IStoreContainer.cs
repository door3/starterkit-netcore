using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Queries;

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
