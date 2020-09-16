using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace D3SK.NetCore.Infrastructure.Stores
{
    public abstract class ProjectionQueryDbStoreContainerBase<T, TStore, TDbStore> :
        ProjectionQueryDbStoreContainerBase<T, int, TStore, TDbStore> 
        where T : class, IEntity<int>
        where TStore : IQueryStore
        where TDbStore : DbContext, TStore
    {
        protected ProjectionQueryDbStoreContainerBase(TDbStore store) : base(store)
        {
        }
    }

    public abstract class ProjectionQueryDbStoreContainerBase<T, TKey, TStore, TDbStore> : QueryDbStoreContainerBase<T, TKey, TStore, TDbStore>,
        IProjectionQueryContainer<T, TKey, TStore>
        where T : class, IEntity<TKey>
        where TStore : IQueryStore
        where TDbStore : DbContext, TStore
    {
        protected ProjectionQueryDbStoreContainerBase(TDbStore store) : base(store)
        {
        }

        public virtual async Task<IList<dynamic>> GetAsync(IProjectionStoreQuery query)
        {
            var items = GetQueryable(query).Project(query);
            if (query.Distinct)
            {
                items = items.Distinct();
            }

            return await items.ToDynamicListAsync();
        }

        public virtual async Task<IList<dynamic>> GetAsync(Expression<Func<T, bool>> predicate,
            Expression<Func<T, dynamic>> selector, bool isDistinct = false)
        {
            var items = DbStore.Set<T>().Where(predicate).Select(selector);
            if (isDistinct)
            {
                items = items.Distinct();
            }

            return await items.ToDynamicListAsync();
        }
    }
}
