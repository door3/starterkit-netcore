using D3SK.NetCore.Common.Stores;
using ExampleBookstore.Services.BookService.Domain.Entities;

namespace ExampleBookstore.Services.BookService.Domain.Stores
{
    public interface IAuthorQueryContainer : IProjectionQueryContainer<Author, IBookQueryStore>
    {
    }

    public interface IAuthorCommandContainer : ICommandContainer<Author, IBookStore>
    {
    }
}
