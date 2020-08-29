using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Features.BookFeatures;
using ExampleBookstore.Services.BookService.Domain.Stores;
using Microsoft.Extensions.Options;

namespace BookService.Infrastructure.Features.BookFeatures
{
    public class BookQuery : EntityQueryBase<IBookDomain, ExampleBookstore.Services.BookService.Domain.Entities.Book, IBookQueryStore, IBookQueryContainer>, IBookQuery
    {
        public BookQuery(IOptions<QueryOptions> queryOptions, IBookQueryContainer queryContainer) 
            : base(queryOptions, queryContainer)
        {
        }
    }
}
