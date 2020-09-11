using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Events
{
    public abstract class AsyncValidatorBase<T, TDomain> : IAsyncValidator<T, TDomain> where TDomain : IDomain
    {
        public abstract Task<bool> IsValidAsync(T item, IDomainInstance<TDomain> domainInstance);

        public Task<bool> IsValidAsync(object item, IDomainInstance domainInstance) =>
            IsValidAsync((T) item, (IDomainInstance<TDomain>) domainInstance);
    }
}
