using System;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;

namespace D3SK.NetCore.Infrastructure.Domain
{
    public abstract class DomainInstanceBase<TDomain> : IDomainInstance<TDomain> where TDomain : IDomain
    {
        public IServiceProvider ServiceProvider { get; }

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

        public virtual Task<TResult> RunFeatureAsync<TDomainRole, TResult>(IAsyncQueryFeature<TDomain, TResult> feature)
            where TDomainRole : IQueryDomainRole<TDomain>
            => GetDomainRole<TDomainRole>().HandleFeatureAsync(this, feature);

        public virtual Task RunFeatureAsync<TDomainRole>(IAsyncCommandFeature<TDomain> feature)
            where TDomainRole : ICommandDomainRole<TDomain>
            => GetDomainRole<TDomainRole>().HandleFeatureAsync(this, feature);

        public virtual async Task<Guid> PublishEventAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEventBase
        {
            switch (domainEvent)
            {
                case IBusEvent busEvent:
                    busEvent.EventGuid = await PublishBusEventAsync(busEvent);
                    return busEvent.EventGuid;
                case IDomainEvent domEvent:
                    await Domain.EventStrategy.HandleEventAsync(domEvent, ServiceProvider);
                    return domEvent.EventGuid;
                default:
                    return Guid.Empty;
            }
        }

        public virtual async Task<bool> ValidateAsync<T>(T item)
        {
            var validationEvent = new DomainValidationEvent<T>(item);
            await Domain.ValidationStrategy.HandleEventAsync(validationEvent, ServiceProvider);
            return validationEvent.IsValid;
        }

        protected virtual async Task<Guid> PublishBusEventAsync<TEvent>(TEvent busEvent) where TEvent : IBusEvent
        {
            return await Domain.Bus.PublishEventAsync(busEvent, this);
        }
    }
}
