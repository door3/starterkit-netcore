using System;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Infrastructure.Domain;
using ExampleBookstore.Domain;
using Microsoft.Extensions.Logging;

namespace ExampleBookstore.Infrastructure
{
    public abstract class ExampleBookstoreDomainInstance<TDomain> : DomainInstanceBase<TDomain>,
        IExampleBookstoreDomainInstance<TDomain>
        where TDomain : IExampleBookstoreDomain<TDomain>
    {
        protected ExampleBookstoreDomainInstance(IServiceProvider serviceProvider, TDomain domain,
            ICurrentUserManager<IUserClaims> currentUserManager, IExceptionManager exceptionManager,
            ILogger<ExampleBookstoreDomainInstance<TDomain>> logger)
            : base(serviceProvider, domain, exceptionManager, logger)
        {
        }
    }
}
