using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Domain;

namespace ExampleBookstore.Services.BookService.Domain.Features
{
    public interface IBookCountQuery : IAsyncQueryFeature<IBookDomain, int>, IFilterable
    {
    }
}
