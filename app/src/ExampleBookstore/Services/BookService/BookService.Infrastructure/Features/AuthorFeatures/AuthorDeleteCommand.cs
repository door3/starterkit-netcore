using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Features.AuthorFeatures;
using ExampleBookstore.Services.BookService.Domain.Stores;

namespace BookService.Infrastructure.Features.AuthorFeatures
{
    public class AuthorDeleteCommand : EntityDeleteCommandBase<IBookDomain, Author, IBookStore, IAuthorCommandContainer>,
        IAuthorDeleteCommand
    {
        public AuthorDeleteCommand(IAuthorCommandContainer commandContainer, IUpdateStrategy updateStrategy)
            : base(commandContainer, updateStrategy)
        {
        }
    }
}