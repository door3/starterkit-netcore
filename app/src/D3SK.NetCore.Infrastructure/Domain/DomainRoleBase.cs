using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Domain;

namespace D3SK.NetCore.Infrastructure.Domain
{
    public abstract class DomainRoleBase<TDomain> : IDomainRole<TDomain> where TDomain : IDomain
    {
        
    }

    public abstract class QueryDomainRoleBase<TDomain> : DomainRoleBase<TDomain>, IQueryDomainRole<TDomain>
        where TDomain : IDomain
    {
        public virtual Task<TResult> HandleFeatureAsync<TResult>(IDomainInstance<TDomain> domainInstance,
            IAsyncQueryFeature<TDomain, TResult> feature)
            => feature.HandleAsync(domainInstance);
    }

    public abstract class CommandDomainRoleBase<TDomain> : DomainRoleBase<TDomain>, ICommandDomainRole<TDomain>
        where TDomain : IDomain
    {
        public virtual Task HandleFeatureAsync(IDomainInstance<TDomain> domainInstance,
            IAsyncCommandFeature<TDomain> feature)
            => feature.HandleAsync(domainInstance);
    }
}
