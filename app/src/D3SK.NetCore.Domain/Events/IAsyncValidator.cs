using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Events
{
    public interface IAsyncValidatorBase
    {
        Task<bool> IsValidAsync(object item, object options, IDomainInstance domainInstance);
    }

    public interface IAsyncValidator<in T, TDomain> : IAsyncValidatorBase where TDomain : IDomain
    {
        Task<bool> IsValidAsync(T item, IDomainInstance<TDomain> domainInstance);
    }

    public interface IAsyncValidator<in T, in TOptions, TDomain> : IAsyncValidatorBase where TDomain : IDomain
    {
        Task<bool> IsValidAsync(T item, TOptions options, IDomainInstance<TDomain> domainInstance);
    }
}