using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Features.BookFeatures;
using ExampleBookstore.Services.BookService.Domain.Stores;

namespace BookService.Infrastructure.Features.BookFeatures
{
    public class BookUpdateCommand : EntityPatchCommandBase<IBookDomain, Book, IBookStore, IBookCommandContainer>,
        IBookUpdateCommand
    {
        public BookUpdateCommand(IBookCommandContainer commandContainer, IUpdateStrategy updateStrategy)
            : base(commandContainer, updateStrategy)
        {
        }

        protected override async Task UpdateDependentsAsync(Book dbItem)
        {
            if (PatchIncludesProperty(nameof(Book.Authors)))
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
}
