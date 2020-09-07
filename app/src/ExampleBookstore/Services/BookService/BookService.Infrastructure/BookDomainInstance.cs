using System;
using D3SK.NetCore.Common.Utilities;
using ExampleBookstore.Infrastructure;
using ExampleBookstore.Services.BookService.Domain;

namespace ExampleBookstore.Services.BookService.Infrastructure
{
    public class BookDomainInstance : ExampleBookstoreDomainInstance<IBookDomain>
    {
        public BookDomainInstance(IServiceProvider serviceProvider, IBookDomain domain,
            ICurrentUserManager<IUserClaims> currentUserManager)
            : base(serviceProvider, domain, currentUserManager)
        {
        }
    }
}