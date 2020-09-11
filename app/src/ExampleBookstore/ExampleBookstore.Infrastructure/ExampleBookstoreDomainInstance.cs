using System;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Infrastructure.Domain;
using ExampleBookstore.Domain;

namespace ExampleBookstore.Infrastructure
{
    public abstract class ExampleBookstoreDomainInstance<TDomain> : DomainInstanceBase<TDomain>,
        IExampleBookstoreDomainInstance<TDomain>
        where TDomain : IExampleBookstoreDomain<TDomain>
    {
        protected ExampleBookstoreDomainInstance(IServiceProvider serviceProvider, TDomain domain,
            ICurrentUserManager<IUserClaims> currentUserManager, IExceptionManager exceptionManager)
            : base(serviceProvider, domain, currentUserManager, exceptionManager)
        {
        }
    }
}