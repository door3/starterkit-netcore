﻿using D3SK.NetCore.Common.Stores;
using ExampleBookstore.Services.BookService.Domain.Entities;

namespace ExampleBookstore.Services.BookService.Domain.Stores
{
    public interface IBookQueryContainer : IProjectionQueryContainer<Book, int, IBookQueryStore>
    {
    }

    public interface IBookCommandContainer : ICommandContainer<Book, int, IBookStore>
    {
    }
}
