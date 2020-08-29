using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Features.Book;
using ExampleBookstore.Services.BookService.Domain.Stores;

namespace BookService.Infrastructure.Features.Book
{
    public class BookCountQuery : EntityCountQueryBase<IBookDomain, ExampleBookstore.Services.BookService.Domain.Entities.Book, IBookQueryStore, IBookQueryContainer>,
        IBookCountQuery
    {
        public BookCountQuery(IBookQueryContainer queryContainer) : base(queryContainer)
        {
        }
    }
}