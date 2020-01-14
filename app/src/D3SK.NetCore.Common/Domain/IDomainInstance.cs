using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Events;

namespace D3SK.NetCore.Common.Domain
{
    public interface IDomainInstance
    {
        Task PublishEventAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;

        Task<bool> ValidateAsync<T>(T item);
    }

    public interface IDomainInstance<TDomain> : IDomainInstance
        where TDomain : IDomain
    {
        TDomain Domain { get; }

        Task<TResult> RunFeatureAsync<TDomainRole, TResult>(IAsyncQueryFeature<TDomain, TResult> feature)
            where TDomainRole : IQueryDomainRole<TDomain>;

        Task RunFeatureAsync<TDomainRole>(IAsyncCommandFeature<TDomain> feature)
            where TDomainRole : ICommandDomainRole<TDomain>;
    }
}
