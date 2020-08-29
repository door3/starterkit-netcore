using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Features.Book;
using ExampleBookstore.Services.BookService.Domain.Stores;

namespace BookService.Infrastructure.Features.Book
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
