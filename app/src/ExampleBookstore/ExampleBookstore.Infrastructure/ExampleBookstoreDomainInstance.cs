using System;
using D3SK.NetCore.Infrastructure.Domain;
using ExampleBookstore.Domain;

namespace ExampleBookstore.Infrastructure
{
    public abstract class ExampleBookstoreDomainInstance<TDomain> : DomainInstanceBase<TDomain>, IExampleBookstoreDomainInstance<TDomain>
        where TDomain : IExampleBookstoreDomain
    {
        protected ExampleBookstoreDomainInstance(IServiceProvider serviceProvider, TDomain domain) 
            : base(serviceProvider, domain)
        {
        }
    }
}
