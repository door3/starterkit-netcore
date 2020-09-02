using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Domain.Events;
using ExampleBookstore.Services.BookService.Domain.Entities;

namespace ExampleBookstore.Services.BookService.Domain.Events
{
    public class ValidateBookEventHandler : AsyncValidationEventHandlerBase<Book>
    {
        public override Task<bool> IsValidAsync(Book item)
        {
            return true.AsTask();
        }
    }
}
