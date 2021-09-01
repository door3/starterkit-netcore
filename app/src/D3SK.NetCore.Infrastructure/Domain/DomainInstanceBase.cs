using System;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace D3SK.NetCore.Infrastructure.Domain
{
    public abstract class DomainInstanceBase<TDomain> : IDomainInstance<TDomain> where TDomain : IDomain<TDomain>, IDomain
    {
        public IServiceProvider ServiceProvider { get; }

        public TDomain Domain { get; }

        public IExceptionManager ExceptionManager { get; }
        private readonly ILogger<DomainInstanceBase<TDomain>> _logger;

        protected DomainInstanceBase(
            IServiceProvider serviceProvider,
            TDomain domain,
            IExceptionManager exceptionManager,
            ILogger<DomainInstanceBase<TDomain>> logger)
        {
            ServiceProvider = serviceProvider.NotNull(nameof(ServiceProvider));
            Domain = domain.NotNull(nameof(domain));
            ExceptionManager = exceptionManager.NotNull(nameof(exceptionManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected virtual TDomainRole GetDomainRole<TDomainRole>()
            where TDomainRole : IDomainRole<TDomain>
        {
            return Domain.GetRole<TDomainRole>();
        }

        // TODO: figure out a better way to fix the circular dependency
        public virtual ICurrentUserManager GetCurrentUserManager() =>
            ServiceProvider.GetRequiredService<ICurrentUserManager>();

        public virtual async Task<TResult> RunFeatureAsync<TDomainRole, TResult>(IAsyncQueryFeature<TDomain, TResult> feature)
            where TDomainRole : IQueryDomainRole<TDomain>
        {
            _logger.LogTrace("Running feature {@feature}", feature);
            TResult result;
            try
            {
                result = await GetDomainRole<TDomainRole>().HandleFeatureAsync(this, feature);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on feature {@feature}", feature);
                throw;
            }
            return result;
        }

        public virtual async Task RunFeatureAsync<TDomainRole>(IAsyncCommandFeature<TDomain> feature)
            where TDomainRole : ICommandDomainRole<TDomain>
        {
            try
            {
                await GetDomainRole<TDomainRole>().HandleFeatureAsync(this, feature);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on feature {@feature}", feature);
                _logger.LogError(ex, "Error details");
                throw;
            }
        }

        public virtual async Task<Guid> PublishEventAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEventBase
        {
            switch (domainEvent)
            {
                case IBusEvent busEvent:
                    busEvent.EventGuid = await PublishBusEventAsync(busEvent);
                    return busEvent.EventGuid;
                case IDomainEvent domEvent:
                    await Domain.EventStrategy.HandleEventAsync(domEvent, ServiceProvider, this);
                    return domEvent.EventGuid;
                default:
                    return Guid.Empty;
            }
        }

        public virtual async Task<bool> ValidateAsync<T>(T item)
        {
            var validationEvent = await Domain.ValidationStrategy.HandleValidationAsync(item, ServiceProvider, this);
            return validationEvent.IsValid;
        }

        public virtual async Task<bool> ValidateAsync<T, TOptions>(T item, TOptions validationOptions)
        {
            var validationEvent = await Domain.ValidationStrategy.HandleValidationAsync(item, validationOptions, ServiceProvider, this);
            return validationEvent.IsValid;
        }

        protected virtual async Task<Guid> PublishBusEventAsync<TEvent>(TEvent busEvent) where TEvent : IBusEvent
        {
            return await Domain.Bus.PublishEventAsync(busEvent, this);
        }
    }
}
