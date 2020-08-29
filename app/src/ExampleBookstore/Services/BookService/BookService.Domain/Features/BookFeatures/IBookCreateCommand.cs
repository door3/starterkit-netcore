﻿using D3SK.NetCore.Domain.Features;

namespace ExampleBookstore.Services.BookService.Domain.Features.BookFeatures
{
    public interface IBookCreateCommand : IEntityCreateCommand<IBookDomain, Entities.Book>
    {
    }
}