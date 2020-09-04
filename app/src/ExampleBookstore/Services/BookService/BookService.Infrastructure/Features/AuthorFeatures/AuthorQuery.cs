using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Features.AuthorFeatures;
using ExampleBookstore.Services.BookService.Domain.Stores;
using Microsoft.Extensions.Options;

namespace ExampleBookstore.Services.BookService.Infrastructure.Features.AuthorFeatures
{
    public class AuthorQuery : EntityQueryBase<IBookDomain, Author, IBookQueryStore, IAuthorQueryContainer>, IAuthorQuery
    {
        public AuthorQuery(IOptions<QueryOptions> queryOptions, IAuthorQueryContainer queryContainer) 
            : base(queryOptions, queryContainer)
        {
        }
    }
}
