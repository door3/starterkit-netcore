using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Features.BookFeatures;
using ExampleBookstore.Services.BookService.Domain.Stores;

namespace BookService.Infrastructure.Features.BookFeatures
{
    public class BookCreateCommand : EntityCreateCommandBase<IBookDomain, ExampleBookstore.Services.BookService.Domain.Entities.Book, IBookStore, IBookCommandContainer>,
        IBookCreateCommand
    {
        public BookCreateCommand(IBookCommandContainer commandContainer, IUpdateStrategy updateStrategy) : base(
            commandContainer, updateStrategy)
        {
        }
    }
}
