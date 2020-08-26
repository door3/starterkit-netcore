using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Infrastructure.Domain;
using ExampleBookstore.Domain;

namespace ExampleBookstore.Infrastructure
{
    public class ExampleBookstoreQueryDomainRole<TDomain> : QueryDomainRoleBase<TDomain>
        where TDomain : IExampleBookstoreDomain
    {
    }

    public class ExampleBookstoreCommandDomainRole<TDomain> : CommandDomainRoleBase<TDomain>
        where TDomain : IExampleBookstoreDomain
    {
    }
}