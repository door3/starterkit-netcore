using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Features.AuthorFeatures;
using ExampleBookstore.Services.BookService.Domain.Stores;

namespace ExampleBookstore.Services.BookService.Infrastructure.Features.AuthorFeatures
{
    public class AuthorCountQuery : EntityCountQueryBase<IBookDomain, Author, IBookQueryStore, IAuthorQueryContainer>,
        IAuthorCountQuery
    {
        public AuthorCountQuery(IAuthorQueryContainer queryContainer) : base(queryContainer)
        {
        }
    }
}