using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;

namespace D3SK.NetCore.Infrastructure.Domain
{
    public abstract class DomainInstanceBase<TDomain> : IDomainInstance<TDomain> where TDomain : IDomain
    {
        protected readonly IServiceProvider ServiceProvider;

        public TDomain Domain { get; }

        protected DomainInstanceBase(
            IServiceProvider serviceProvider,
            TDomain domain)
        {
            ServiceProvider = serviceProvider.NotNull(nameof(ServiceProvider));
            Domain = domain.NotNull(nameof(domain));
        }

        protected virtual TDomainRole GetDomainRole<TDomainRole>()
            where TDomainRole : IDomainRole<TDomain>
        {
            return Domain.GetRole<TDomainRole>();
        }

        public Task<TResult> RunFeatureAsync<TDomainRole, TResult>(IAsyncQueryFeature<TDomain, TResult> feature)
            where TDomainRole : IQueryDomainRole<TDomain>
            => GetDomainRole<TDomainRole>().HandleFeatureAsync(this, feature);

        public Task RunFeatureAsync<TDomainRole>(IAsyncCommandFeature<TDomain> feature) where TDomainRole : ICommandDomainRole<TDomain>
            => GetDomainRole<TDomainRole>().HandleFeatureAsync(this, feature);

        public Task<Guid> PublishEventAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateAsync<T>(T item)
        {
            throw new NotImplementedException();
        }
    }
}
