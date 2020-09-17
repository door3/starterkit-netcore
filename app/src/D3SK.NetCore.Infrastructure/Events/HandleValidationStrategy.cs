using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace D3SK.NetCore.Infrastructure.Events
{
    public class HandleValidationStrategy<TDomain> : IHandleValidationStrategy<TDomain>
        where TDomain : IDomain
    {
        public IList<ValidationHandlerInfo> ValidationHandlers { get; } = new List<ValidationHandlerInfo>();

        public void AddAsyncHandler<THandler, T>(HandleDomainEventOptions options = null)
            where THandler : class, IAsyncValidator<T, TDomain>
        {
            ValidationHandlers.Add(new ValidationHandlerInfo(typeof(T), typeof(THandler), null, options));
        }

        public void AddAsyncHandler<THandler, T, TOptions>(HandleDomainEventOptions options = null)
            where THandler : class, IAsyncValidator<T, TOptions, TDomain>
        {
            ValidationHandlers.Add(new ValidationHandlerInfo(typeof(T), typeof(THandler), typeof(TOptions), options));
        }

        public Task<IValidationEvent<T>> HandleValidationAsync<T>(T objToValidate,
            IServiceProvider serviceProvider, IDomainInstance<TDomain> domainInstance)
        {
            return HandleValidationCoreAsync(objToValidate, null, serviceProvider, domainInstance);
        }

        public Task<IValidationEvent<T>> HandleValidationAsync<T, TOptions>(T objToValidate, TOptions validationOptions,
            IServiceProvider serviceProvider, IDomainInstance<TDomain> domainInstance)
        {
            return HandleValidationCoreAsync(objToValidate, validationOptions, serviceProvider, domainInstance);
        }

        private async Task<IValidationEvent<T>> HandleValidationCoreAsync<T>(T objToValidate, object validationOptions, 
            IServiceProvider serviceProvider, IDomainInstance<TDomain> domainInstance)
        {
            var eventType = objToValidate.GetType();

            foreach (var type in eventType.GetBaseTypes(true))
            {
                var handlers = ValidationHandlers.Where(x =>
                    x.ObjectType == type && (validationOptions == null ||
                                             x.ValidationOptionsType == validationOptions?.GetType()));
                foreach (var handler in handlers)
                {
                    if (!handler.HandlerType?.ImplementsInterface<IAsyncValidatorBase>() ?? true)
                    {
                        continue;
                    }

                    var eventHandler = (IAsyncValidatorBase)ActivatorUtilities.CreateInstance(
                        serviceProvider,
                        handler.HandlerType);
                    var isValid = await eventHandler.IsValidAsync(objToValidate, validationOptions, domainInstance);

                    if (!isValid)
                    {
                        return new DomainValidationEvent<T>(objToValidate, false);
                    }
                }
            }

            return new DomainValidationEvent<T>(objToValidate, true);
        }
    }
}
