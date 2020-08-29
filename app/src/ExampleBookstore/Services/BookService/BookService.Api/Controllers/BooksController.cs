using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using D3SK.NetCore.Api.Controllers;
using D3SK.NetCore.Api.Models;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Extensions;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Features;
using ExampleBookstore.Services.BookService.Domain.Features.BookFeatures;
using Microsoft.AspNetCore.Mvc;

namespace ExampleBookstore.Services.BookService.Api.Controllers
{
    public class BooksController : EntityPatchControllerBase<IBookDomain, Book,
        IBookCountQuery, IBookQuery, IBookProjectionQuery, IBookCreateCommand, IBookUpdateCommand, IBookDeleteCommand,
        IBookUpdateCommand>
    {
        public BooksController(IDomainInstance<IBookDomain> instance, IExceptionManager exceptionManager) : base(
            instance, exceptionManager)
        {
        }
    }
}