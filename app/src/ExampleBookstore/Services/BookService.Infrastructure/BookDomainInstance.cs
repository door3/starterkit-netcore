using System;
using System.Collections.Generic;
using System.Text;
using ExampleBookstore.Domain;
using ExampleBookstore.Infrastructure;
using ExampleBookstore.Services.BookService.Domain;

namespace BookService.Infrastructure
{
    public class BookDomainInstance : ExampleBookstoreDomainInstance<IBookDomain>
    {
        public BookDomainInstance(IServiceProvider serviceProvider, IBookDomain domain) 
            : base(serviceProvider, domain)
        {
        }
    }
}
