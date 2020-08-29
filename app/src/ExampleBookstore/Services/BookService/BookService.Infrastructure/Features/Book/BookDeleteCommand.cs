using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Features;
using ExampleBookstore.Services.BookService.Domain.Stores;

namespace BookService.Infrastructure.Features
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