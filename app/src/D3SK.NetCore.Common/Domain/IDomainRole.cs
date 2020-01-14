using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Domain
{
    public interface IDomainRole
    {
    }

    public interface IDomainRole<TDomain> : IDomainRole where TDomain : IDomain
    {
    }

    public interface IQueryDomainRole<TDomain> : IDomainRole<TDomain> where TDomain : IDomain
    {
    }

    public interface ICommandDomainRole<TDomain> : IDomainRole<TDomain> where TDomain : IDomain
    {
    }
}
