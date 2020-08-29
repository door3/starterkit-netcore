using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Features.Book;
using ExampleBookstore.Services.BookService.Domain.Stores;

namespace BookService.Infrastructure.Features.Book
{
    public class BookUpdateCommand : EntityUpdateCommandBase<IBookDomain, ExampleBookstore.Services.BookService.Domain.Entities.Book, IBookStore, IBookCommandContainer>,
        IBookUpdateCommand
    {
        public BookUpdateCommand(IBookCommandContainer commandContainer, IUpdateStrategy updateStrategy)
            : base(commandContainer, updateStrategy)
        {
        }

        protected override async Task UpdateDependentsAsync(ExampleBookstore.Services.BookService.Domain.Entities.Book dbItem)
        {
            await UpdateStrategy.UpdateCollectionAsync(
                CurrentItem.Authors,
                OriginalItem?.Authors,
                dbItem.Authors,
                onAddItem: item => TaskAction.Run(() => dbItem.AddAuthor(item.AuthorId)),
                onDeleteItem: item => TaskAction.Run(() => dbItem.RemoveAuthor(item.AuthorId)),
                getItemId: item => new {item.BookId, item.AuthorId});
        }
    }
}
