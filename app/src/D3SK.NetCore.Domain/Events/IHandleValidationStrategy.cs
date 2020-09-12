using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Events
{
    public interface IHandleValidationStrategy<TDomain>
        where TDomain : IDomain
    {
        IList<DomainEventHandlerInfo> ValidationHandlers { get; }

        void AddAsyncHandler<THandler, T>(HandleDomainEventOptions options = null)
            where THandler : class, IAsyncValidator<T, TDomain>;

        Task<IValidationEvent<T>> HandleValidationAsync<T>(T objToValidate, IServiceProvider serviceProvider,
            IDomainInstance<TDomain> domainInstance);
    }
}
