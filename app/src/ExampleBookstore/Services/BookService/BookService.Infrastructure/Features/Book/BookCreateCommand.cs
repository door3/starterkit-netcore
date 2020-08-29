using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Features;
using ExampleBookstore.Services.BookService.Domain.Stores;

namespace BookService.Infrastructure.Features
{
    public class BookCreateCommand : EntityCreateCommandBase<IBookDomain, Book, IBookStore, IBookCommandContainer>,
        IBookCreateCommand
    {
        public BookCreateCommand(IBookCommandContainer commandContainer, IUpdateStrategy updateStrategy) : base(
            commandContainer, updateStrategy)
        {
        }
    }
}
