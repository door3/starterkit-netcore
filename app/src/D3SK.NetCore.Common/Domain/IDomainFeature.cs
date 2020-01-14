using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3SK.NetCore.Common.Domain
{
    public interface IDomainFeature
    {
    }

    public interface IQueryFeature : IDomainFeature
    {
    }

    public interface ICommandFeature : IDomainFeature
    {
    }

    public interface IDomainFeature<TDomain> : IDomainFeature where TDomain : IDomain
    {
    }
    
    public interface IAsyncQueryFeature<TDomain, TResult> : IDomainFeature<TDomain>, IQueryFeature
        where TDomain : IDomain
    {
        Task<TResult> HandleAsync(IDomainInstance<TDomain> domainInstance);
    }

    public interface IAsyncCommandFeature<TDomain> : IDomainFeature<TDomain>, ICommandFeature
        where TDomain : IDomain
    {
        Task HandleAsync(IDomainInstance<TDomain> domainInstance);
    }
}
