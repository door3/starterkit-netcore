using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using D3SK.NetCore.Api.Controllers;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Features.AuthorFeatures;

namespace ExampleBookstore.Services.BookService.Api.Controllers
{
    public class AuthorsController : EntityPatchControllerBase<IBookDomain, Author,
        IAuthorCountQuery, IAuthorQuery, IAuthorProjectionQuery, IAuthorCreateCommand, IAuthorUpdateCommand, IAuthorDeleteCommand,
        IAuthorUpdateCommand>
    {
        public AuthorsController(IDomainInstance<IBookDomain> instance, IExceptionManager exceptionManager) : base(
            instance, exceptionManager)
        {
        }
    }
}
