﻿using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Features.BookFeatures;
using ExampleBookstore.Services.BookService.Domain.Stores;

namespace ExampleBookstore.Services.BookService.Infrastructure.Features.BookFeatures
{
    public class BookUpdateCommand : EntityPatchCommandBase<IBookDomain, Book, IBookStore, IBookCommandContainer>,
        IBookUpdateCommand
    {
        public BookUpdateCommand(IBookCommandContainer commandContainer, IUpdateStrategy updateStrategy)
            : base(commandContainer, updateStrategy)
        {
        }

        protected override async Task UpdateDependentsAsync(Book dbStoreItem)
        {
            if (PatchIncludesProperty(nameof(Book.Authors)))
            {
                await UpdateStrategy.UpdateCollectionAsync(
                    CurrentItem.Authors,
                    OriginalItem?.Authors,
                    dbStoreItem.Authors,
                    onAddItem: item => TaskAction.Run(() => dbStoreItem.AddAuthor(item.AuthorId)),
                    onDeleteItem: item => TaskAction.Run(() => dbStoreItem.RemoveAuthor(item.AuthorId)),
                    getItemId: item => new {item.BookId, item.AuthorId});
            }
        }
    }
}
