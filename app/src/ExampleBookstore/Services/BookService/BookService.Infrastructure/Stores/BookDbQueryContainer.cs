using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Infrastructure.Extensions;
using D3SK.NetCore.Infrastructure.Stores;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Stores;
using Microsoft.EntityFrameworkCore;

namespace BookService.Infrastructure.Stores
{
    public class BookDbQueryContainer :
        ProjectionQueryDbStoreContainerBase<Book, IBookQueryStore, BookDbStore>, IBookQueryContainer
    {
        public BookDbQueryContainer(BookDbStore store) : base(store)
        {
        }

        protected override async Task<Book> LoadRelationsAsync(Book item, string includes = null)
        {
            if (includes == StoreQueryIncludes.None)
                return item;

            await DbStore.Entry(item).Collection(x => x.Authors).Query().Include(x => x.Author).LoadAsync();
            return item;
        }

        protected override IQueryable<Book> WithIncludes(IQueryable<Book> items, string includes = null)
        {
            if (!includes.HasIncludes())
                return items;

            items = items.Include(x => x.Authors).ThenInclude(x => x.Author);

            return items;
        }
    }
}