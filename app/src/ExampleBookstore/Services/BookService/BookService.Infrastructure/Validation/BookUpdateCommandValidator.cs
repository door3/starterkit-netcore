using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;
using D3SK.NetCore.Domain.Models;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Infrastructure.Features.BookFeatures;

namespace ExampleBookstore.Services.BookService.Infrastructure.Validation
{
    public class BookUpdateCommandValidator : AsyncValidatorBase<BookUpdateCommand, IBookDomain>
    {
        public override async Task<bool> IsValidAsync(BookUpdateCommand item, IDomainInstance<IBookDomain> domainInstance)
        {
            var bookIsValid = await domainInstance.ValidateAsync(item.CurrentItem, 
                new PatchEntityValidationOptions(item.PropertiesToUpdate));
            return bookIsValid;
        }
    }
}
