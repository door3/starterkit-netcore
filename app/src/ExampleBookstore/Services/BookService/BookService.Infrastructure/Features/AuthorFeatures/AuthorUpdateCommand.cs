using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Features.AuthorFeatures;
using ExampleBookstore.Services.BookService.Domain.Stores;

namespace ExampleBookstore.Services.BookService.Infrastructure.Features.AuthorFeatures
{
    public class AuthorUpdateCommand : EntityPatchCommandBase<IBookDomain, Author, IBookStore, IAuthorCommandContainer>,
        IAuthorUpdateCommand
    {
        public AuthorUpdateCommand(IAuthorCommandContainer commandContainer, IUpdateStrategy updateStrategy)
            : base(commandContainer, updateStrategy)
        {
        }
    }
}
