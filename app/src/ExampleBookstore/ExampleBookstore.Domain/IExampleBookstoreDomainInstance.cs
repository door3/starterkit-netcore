using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Domain;

namespace ExampleBookstore.Domain
{
    public interface IExampleBookstoreDomainInstance<TDomain> : IDomainInstance<TDomain> where TDomain : IExampleBookstoreDomain
    {
    }
}
