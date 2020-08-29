using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Features.BookFeatures;
using ExampleBookstore.Services.BookService.Domain.Stores;

namespace BookService.Infrastructure.Features.BookFeatures
{
    public class BookDeleteCommand : EntityDeleteCommandBase<IBookDomain, Book, IBookStore, IBookCommandContainer>,
        IBookDeleteCommand
    {
        public BookDeleteCommand(IBookCommandContainer commandContainer, IUpdateStrategy updateStrategy)
            : base(commandContainer, updateStrategy)
        {
        }
    }
}