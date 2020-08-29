﻿using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Infrastructure.Features;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Features;
using ExampleBookstore.Services.BookService.Domain.Stores;
using Microsoft.Extensions.Options;

namespace BookService.Infrastructure.Features
{
    public class BookCountQuery : EntityCountQueryBase<IBookDomain, Book, IBookQueryStore, IBookQueryContainer>,
        IBookCountQuery
    {
        public BookCountQuery(IBookQueryContainer queryContainer) : base(queryContainer)
        {
        }
    }
}