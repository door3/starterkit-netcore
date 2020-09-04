using D3SK.NetCore.Infrastructure.Stores;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Stores;

namespace ExampleBookstore.Services.BookService.Infrastructure.Stores
{
    public class BookDbCommandContainer : CommandDbStoreContainerBase<Book, IBookStore, BookDbStore>,
        IBookCommandContainer
    {
        public BookDbCommandContainer(BookDbStore store) : base(store)
        {
        }
    }
}
