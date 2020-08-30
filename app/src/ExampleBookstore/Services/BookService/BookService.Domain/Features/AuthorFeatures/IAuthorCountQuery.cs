﻿using D3SK.NetCore.Domain.Features;
using ExampleBookstore.Services.BookService.Domain.Entities;

namespace ExampleBookstore.Services.BookService.Domain.Features.AuthorFeatures
{
    public interface IAuthorCountQuery : IEntityCountQuery<IBookDomain, Author>
    {
    }
}
