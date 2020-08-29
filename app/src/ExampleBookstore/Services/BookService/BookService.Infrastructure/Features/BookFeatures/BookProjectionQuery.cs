using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Features.BookFeatures;
using ExampleBookstore.Services.BookService.Domain.Stores;
using Microsoft.Extensions.Options;

namespace BookService.Infrastructure.Features.BookFeatures
{
    public class BookProjectionQuery :
        EntityProjectionQueryBase<IBookDomain, ExampleBookstore.Services.BookService.Domain.Entities.Book, IBookQueryStore, IBookQueryContainer>, IBookProjectionQuery
    {
        public BookProjectionQuery(IOptions<QueryOptions> queryOptions, IBookQueryContainer queryContainer) 
            : base(queryOptions, queryContainer)
        {
        }
    }
}