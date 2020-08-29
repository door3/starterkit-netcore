using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace D3SK.NetCore.Infrastructure.Stores
{
    public abstract class QueryDbStoreContainerBase<T, TStore, TDbStore> : QueryDbStoreContainerBase<T, int, TStore, TDbStore>
        where T : class, IEntity<int>
        where TStore : IQueryStore
        where TDbStore : DbContext, TStore
    {
        protected QueryDbStoreContainerBase(TDbStore store) : base(store)
        {
        }
    }

    public abstract class QueryDbStoreContainerBase<T, TKey, TStore, TDbStore> : DbStoreContainerBase<TStore, TDbStore>, IQueryContainer<T, TKey, TStore>
        where T : class, IEntity<TKey>
        where TStore : IQueryStore
        where TDbStore : DbContext, TStore
    {
        protected QueryDbStoreContainerBase(TDbStore store) : base(store)
        {
        }

        public virtual async Task<int> CountAsync(IFilterable filterInfo = null)
        {
            var items = DbStore.Set<T>()
                .AsNoTracking()
                .Filter(filterInfo);

            return await items.CountAsync();
        }

        public virtual async Task<T> GetAsync(TKey id, string includes = null, bool isTracked = true)
        {
            var item = await DbStore.Set<T>().FindAsync(id);
            return item != null ? await LoadRelationsAsync(item, includes) : null;
        }

        public virtual async Task<IList<T>> GetAsync(IStoreQuery query = null)
        {
            var items = GetQueryable(query);
            return await items.ToListAsync();
        }

        public virtual async Task<IList<T>> GetAsync(Expression<Func<T, bool>> predicate, string includes = null, bool isTracked = false)
        {
            var items = DbStore.Set<T>().Where(predicate).AsNoTracking();
            return await WithIncludes(items, includes).ToListAsync();
        }

        protected virtual Task<T> LoadRelationsAsync(T item, string includes = null)
        {
            return Task.FromResult(item);
        }

        protected virtual IQueryable<T> WithIncludes(IQueryable<T> items, string includes = null)
        {
            return items;
        }

        protected virtual IQueryable<T> GetQueryable(IStoreQuery query = null)
        {
            var items = DbStore.Set<T>()
                .Sort(query)
                .Filter(query)
                .Page(query);
            if (!(query?.TrackEntities).GetValueOrDefault()) items = items.AsNoTracking();

            return WithIncludes(items, query.GetIncludes());
        }
    }

}
