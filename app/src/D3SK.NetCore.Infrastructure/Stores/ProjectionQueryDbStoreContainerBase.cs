﻿using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace D3SK.NetCore.Infrastructure.Stores
{

    public abstract class ProjectionQueryDbStoreContainerBase<T, TStore, TDbStore> : QueryDbStoreContainerBase<T, TStore, TDbStore>,
        IProjectionQueryContainer<T, TStore>
        where T : class, IEntity<int>
        where TStore : IQueryStore
        where TDbStore : DbContext, TStore
    {
        protected ProjectionQueryDbStoreContainerBase(TDbStore store) : base(store)
        {
        }

        public virtual async Task<IList<object>> GetAsync(IProjectionStoreQuery query)
        {
            var items = GetQueryable(query).Project(query);
            if (query.Distinct)
            {
                items = items.Distinct();
            }

            return await items.ToDynamicListAsync();
        }

    }
}