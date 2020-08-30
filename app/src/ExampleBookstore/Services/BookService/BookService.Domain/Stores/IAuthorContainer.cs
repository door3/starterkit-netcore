using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Stores;
using ExampleBookstore.Services.BookService.Domain.Entities;

namespace ExampleBookstore.Services.BookService.Domain.Stores
{
    public interface IAuthorQueryContainer : IProjectionQueryContainer<Author, int, IBookQueryStore>
    {
    }

    public interface IAuthorCommandContainer : ICommandContainer<Author, int, IBookStore>
    {
    }
}
