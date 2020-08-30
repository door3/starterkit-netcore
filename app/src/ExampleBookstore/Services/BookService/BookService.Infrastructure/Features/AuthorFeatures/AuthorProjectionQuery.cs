using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Features.AuthorFeatures;
using ExampleBookstore.Services.BookService.Domain.Stores;
using Microsoft.Extensions.Options;

namespace BookService.Infrastructure.Features.AuthorFeatures
{
    public class AuthorProjectionQuery :
        EntityProjectionQueryBase<IBookDomain, Author, IBookQueryStore, IAuthorQueryContainer>, IAuthorProjectionQuery
    {
        public AuthorProjectionQuery(IOptions<QueryOptions> queryOptions, IAuthorQueryContainer queryContainer) 
            : base(queryOptions, queryContainer)
        {
        }
    }
}