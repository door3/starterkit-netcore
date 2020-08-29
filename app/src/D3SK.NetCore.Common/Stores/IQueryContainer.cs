using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Queries;

namespace D3SK.NetCore.Common.Stores
{

    public interface IQueryContainer : IStoreContainer
    {
    }

    public interface IQueryContainer<out TStore> : IStoreContainer<TStore>, IQueryContainer where TStore : IQueryStore
    {
    }

    public interface IQueryContainer<T, out TStore> : IQueryContainer<TStore>
        where T : IEntityBase
        where TStore : IQueryStore
    {
        Task<int> CountAsync(IFilterable filterInfo = null);

        Task<IList<T>> GetAsync(Expression<Func<T, bool>> predicate, string includes = null, bool isTracked = false);

        Task<IList<T>> GetAsync(IStoreQuery query = null);
    }

    public interface IQueryContainer<T, in TKey, out TStore> : IQueryContainer<T, TStore>
        where T : IEntity<TKey>
        where TStore : IQueryStore
    {
        Task<T> GetAsync(TKey id, string includes = null, bool isTracked = true);
    }

    public interface IProjectionQueryContainer<T, out TStore> : IQueryContainer<T, TStore>
        where T : IEntityBase
        where TStore : IQueryStore
    {
        Task<IList<dynamic>> GetAsync(IProjectionStoreQuery query);

        Task<IList<dynamic>> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int, dynamic>> selector);
    }

    public interface IProjectionQueryContainer<T, in TKey, out TStore> : IProjectionQueryContainer<T, TStore>,
        IQueryContainer<T, TKey, TStore>
        where T : IEntity<TKey>
        where TStore : IQueryStore
    {
    }
}
