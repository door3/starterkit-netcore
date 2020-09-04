using D3SK.NetCore.Infrastructure.Stores;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Stores;

namespace ExampleBookstore.Services.BookService.Infrastructure.Stores
{
    public class AuthorDbCommandContainer : CommandDbStoreContainerBase<Author, IBookStore, BookDbStore>,
        IAuthorCommandContainer
    {
        public AuthorDbCommandContainer(BookDbStore store) : base(store)
        {
        }
    }
}
