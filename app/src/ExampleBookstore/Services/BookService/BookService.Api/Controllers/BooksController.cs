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
using Microsoft.AspNetCore.Mvc;

namespace ExampleBookstore.Services.BookService.Api.Controllers
{
    public class BooksController : DomainControllerBase<IBookDomain>
    {
        public BooksController(IDomainInstance<IBookDomain> instance, IExceptionManager exceptionManager) : base(
            instance, exceptionManager)
        {
        }

        [HttpGet("{id}", Name = nameof(GetBook))]
        public async Task<Book> GetBook([FromServices] IBookQuery query, [FromRoute] int id)
        {
            query.Filters.Add(new QueryFilter(id));
            var result = (await DomainInstance.RunQueryAsync(query)).SingleOrDefault();
            return result;
        }

        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        public async Task<IActionResult> CreateBook([FromServices] IBookCreateCommand command,
            [FromBody] EntityCreateCommandRequest<Book> request, ApiVersion version)
        {
            request.SetCommand(command);
            await DomainInstance.RunCommandAsync(command);
            return CreatedAtRoute(nameof(GetBook), new {id = command.CurrentItem.Id, version = $"{version}"},
                request.CurrentItem);
        }
    }
}
