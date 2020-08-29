using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Features;
using ExampleBookstore.Services.BookService.Domain.Entities;

namespace ExampleBookstore.Services.BookService.Domain.Features
{
    public interface IBookCountQuery : IEntityCountQuery<IBookDomain, Book>
    {
    }
}
