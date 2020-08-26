using System.Threading.Tasks;

namespace D3SK.NetCore.Domain
{
    public interface IDomainRole
    {
    }

    public interface IDomainRole<TDomain> : IDomainRole where TDomain : IDomain
    {
    }

    public interface IQueryDomainRole<TDomain> : IDomainRole<TDomain> where TDomain : IDomain
    {
        Task<TResult> HandleFeatureAsync<TResult>(IDomainInstance<TDomain> domainInstance,
            IAsyncQueryFeature<TDomain, TResult> feature);
    }

    public interface ICommandDomainRole<TDomain> : IDomainRole<TDomain> where TDomain : IDomain
    {
        Task HandleFeatureAsync(IDomainInstance<TDomain> domainInstance, IAsyncCommandFeature<TDomain> feature);
    }
}
