using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Features.AuthorFeatures;
using ExampleBookstore.Services.BookService.Domain.Features.BookFeatures;
using ExampleBookstore.Services.BookService.Domain.Stores;

namespace BookService.Infrastructure.Features.BookFeatures
{
    public class BookCountQuery : EntityCountQueryBase<IBookDomain, Book, IBookQueryStore, IBookQueryContainer>,
        IBookCountQuery
    {
        public BookCountQuery(IBookQueryContainer queryContainer) : base(queryContainer)
        {
        }
    }
}