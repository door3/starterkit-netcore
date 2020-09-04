using System.Linq;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Infrastructure.Extensions;
using D3SK.NetCore.Infrastructure.Stores;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Stores;
using Microsoft.EntityFrameworkCore;

namespace ExampleBookstore.Services.BookService.Infrastructure.Stores
{
    public class AuthorDbQueryContainer :
        ProjectionQueryDbStoreContainerBase<Author, IBookQueryStore, BookDbStore>, IAuthorQueryContainer
    {
        public AuthorDbQueryContainer(BookDbStore store) : base(store)
        {
        }

        protected override async Task<Author> LoadRelationsAsync(Author item, string includes = null)
        {
            if (includes == StoreQueryIncludes.None)
                return item;

            await DbStore.Entry(item).Collection(x => x.Books).Query()
                .Include(x => x.Book).LoadAsync();
            return item;
        }

        protected override IQueryable<Author> WithIncludes(IQueryable<Author> items, string includes = null)
        {
            if (!includes.HasIncludes())
                return items;

            if (includes.HasInclude(StoreQueryIncludes.Full))
            {
                items = items.Include(x => x.Books).ThenInclude(x => x.Book);
            }

            return items;
        }
    }
}
