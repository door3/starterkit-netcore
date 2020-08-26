using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Infrastructure.Domain;
using ExampleBookstore.Domain;

namespace ExampleBookstore.Infrastructure
{
    public class ExampleBookstoreDomainInstance<TDomain> : DomainInstanceBase<TDomain>, IExampleBookstoreDomainInstance<TDomain>
        where TDomain : IExampleBookstoreDomain
    {
        public ExampleBookstoreDomainInstance(IServiceProvider serviceProvider, TDomain domain) 
            : base(serviceProvider, domain)
        {
        }
    }
}
