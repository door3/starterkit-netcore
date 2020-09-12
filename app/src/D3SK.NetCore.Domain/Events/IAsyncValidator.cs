using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Events
{
    public interface IAsyncValidatorBase
    {
        Task<bool> IsValidAsync(object item, IDomainInstance domainInstance);
    }

    public interface IAsyncValidator<in T, TDomain> : IAsyncValidatorBase where TDomain : IDomain
    {
        Task<bool> IsValidAsync(T item, IDomainInstance<TDomain> domainInstance);
    }
}