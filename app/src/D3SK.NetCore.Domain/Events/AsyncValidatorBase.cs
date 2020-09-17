using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Events
{
    public abstract class AsyncValidatorBase<T, TDomain> : IAsyncValidator<T, TDomain> where TDomain : IDomain
    {
        public abstract Task<bool> IsValidAsync(T item, IDomainInstance<TDomain> domainInstance);

        public Task<bool> IsValidAsync(object item, object options, IDomainInstance domainInstance) =>
            IsValidAsync((T) item, (IDomainInstance<TDomain>) domainInstance);
    }

    public abstract class AsyncValidatorBase<T, TOptions, TDomain> : IAsyncValidator<T, TOptions, TDomain>
        where TDomain : IDomain
        where TOptions : class, new()
    {
        public abstract Task<bool> IsValidAsync(T item, TOptions options, IDomainInstance<TDomain> domainInstance);

        public Task<bool> IsValidAsync(object item, object options, IDomainInstance domainInstance) =>
            IsValidAsync((T)item, (TOptions)options ?? new TOptions(), (IDomainInstance<TDomain>)domainInstance);
    }
}
