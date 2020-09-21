using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Queries;

namespace D3SK.NetCore.Common.Stores
{

    public interface IQueryContainerBase : IStoreContainer
    {
    }

    public interface IQueryContainerBase<out TStore> : IStoreContainer<TStore>, IQueryContainerBase where TStore : IQueryStore
    {
    }

    public interface IQueryContainerBase<T, out TStore> : IQueryContainerBase<TStore>
        where T : IEntityBase
        where TStore : IQueryStore
    {
        Task<int> CountAsync(IFilterable filterInfo = null);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task<IList<T>> GetAsync(Expression<Func<T, bool>> predicate, string includes = null, bool isTracked = false);

        Task<IList<T>> GetAsync(IStoreQuery query = null);
    }

    public interface IQueryContainer<T, out TStore> : IQueryContainer<T, int, TStore>
        where T : IEntity<int>
        where TStore : IQueryStore
    {
    }

    public interface IQueryContainer<T, in TKey, out TStore> : IQueryContainerBase<T, TStore>
        where T : IEntity<TKey>
        where TStore : IQueryStore
    {
        Task<T> GetAsync(TKey id, string includes = null, bool isTracked = true);
    }

    public interface IProjectionQueryContainerBase<T, out TStore> : IQueryContainerBase<T, TStore>
        where T : IEntityBase
        where TStore : IQueryStore
    {
        Task<IList<dynamic>> GetAsync(IProjectionStoreQuery query);

        Task<IList<dynamic>> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, dynamic>> selector, bool isDistinct = false);
    }

    public interface IProjectionQueryContainer<T, out TStore> : IProjectionQueryContainer<T, int, TStore>
        where T : IEntity<int>
        where TStore : IQueryStore
    {
    }

    public interface IProjectionQueryContainer<T, in TKey, out TStore> : IProjectionQueryContainerBase<T, TStore>,
        IQueryContainer<T, TKey, TStore>
        where T : IEntity<TKey>
        where TStore : IQueryStore
    {
    }
}
