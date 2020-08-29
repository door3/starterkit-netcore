using D3SK.NetCore.Common.Stores;

namespace ExampleBookstore.Services.BookService.Domain.Stores
{
    public interface IBookQueryStore : IQueryStore
    {
    }

    public interface IBookStore : ITransactionStore
    {
    }
}
