using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Infrastructure.Features.BookFeatures;

namespace ExampleBookstore.Services.BookService.Infrastructure.Validation
{
    public class BookCreateCommandValidator : AsyncValidatorBase<BookCreateCommand, IBookDomain>
    {
        public override async Task<bool> IsValidAsync(BookCreateCommand item, IDomainInstance<IBookDomain> domainInstance)
        {
            var bookIsValid = await domainInstance.ValidateAsync(item.CurrentItem);
            return bookIsValid;
        }
    }
}
