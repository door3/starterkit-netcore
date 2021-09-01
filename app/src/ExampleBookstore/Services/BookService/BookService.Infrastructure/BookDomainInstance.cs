using System;
using D3SK.NetCore.Common.Utilities;
using ExampleBookstore.Infrastructure;
using ExampleBookstore.Services.BookService.Domain;
using Microsoft.Extensions.Logging;

namespace ExampleBookstore.Services.BookService.Infrastructure
{
    public class BookDomainInstance : ExampleBookstoreDomainInstance<IBookDomain>
    {
        public BookDomainInstance(IServiceProvider serviceProvider, IBookDomain domain,
            ICurrentUserManager<IUserClaims> currentUserManager, IExceptionManager exceptionManager,
            ILogger<ExampleBookstoreDomainInstance<IBookDomain>> logger)
            : base(serviceProvider, domain, currentUserManager, exceptionManager, logger)
        {
        }
    }
}
