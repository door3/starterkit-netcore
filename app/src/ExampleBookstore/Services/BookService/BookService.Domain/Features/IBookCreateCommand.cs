using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Domain.Features;
using ExampleBookstore.Services.BookService.Domain.Entities;

namespace ExampleBookstore.Services.BookService.Domain.Features
{
    public interface IBookCreateCommand : IEntityCreateCommand<IBookDomain, Book>
    {
    }
}
