using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Events
{
    public interface IHandleValidationStrategy<TDomain>
        where TDomain : IDomain
    {
        IList<ValidationHandlerInfo> ValidationHandlers { get; }

        void AddAsyncHandler<THandler, T>(HandleDomainEventOptions options = null)
            where THandler : class, IAsyncValidator<T, TDomain>;

        void AddAsyncHandler<THandler, T, TOptions>(HandleDomainEventOptions options = null)
            where THandler : class, IAsyncValidator<T, TOptions, TDomain>;

        Task<IValidationEvent<T>> HandleValidationAsync<T>(T objToValidate, IServiceProvider serviceProvider,
            IDomainInstance<TDomain> domainInstance);

        Task<IValidationEvent<T>> HandleValidationAsync<T, TOptions>(T objToValidate, TOptions validationOptions, 
            IServiceProvider serviceProvider, IDomainInstance<TDomain> domainInstance);
    }
}
