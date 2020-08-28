using System;
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
    public class BookQuery : EntityQueryBase<IBookDomain, Book, IBookQueryStore, IBookQueryContainer>, IBookQuery
    {
        public BookQuery(IOptions<QueryOptions> queryOptions, IBookQueryContainer queryContainer) : base(queryOptions,
            queryContainer)
        {
        }
    }
}
