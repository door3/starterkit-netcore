using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Domain.Features;
using D3SK.NetCore.Domain.Stores;

namespace ExampleBookstore.Services.BookService.Domain.Features
{
    public interface IBookProjectionQuery : IEntityProjectionQuery<IBookDomain>
    {
    }
}
