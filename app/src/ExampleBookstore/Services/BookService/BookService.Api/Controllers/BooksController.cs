using D3SK.NetCore.Api.Controllers;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Features.BookFeatures;

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