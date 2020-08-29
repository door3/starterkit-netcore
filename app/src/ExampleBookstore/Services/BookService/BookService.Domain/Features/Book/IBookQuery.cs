using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Features;
using D3SK.NetCore.Domain.Stores;
using ExampleBookstore.Services.BookService.Domain.Entities;

namespace ExampleBookstore.Services.BookService.Domain.Features
{
    public interface IBookQuery : IEntityQuery<IBookDomain, Book>
    {
    }
}
