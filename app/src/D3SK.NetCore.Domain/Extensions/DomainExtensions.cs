using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Extensions
{
    public static class DomainExtensions
    {
        public static Task<TResult> RunQueryAsync<TDomain, TResult>(this IDomainInstance<TDomain> source,
            IAsyncQueryFeature<TDomain, TResult> feature) where TDomain : IDomain
        {
            return source.RunFeatureAsync<IQueryDomainRole<TDomain>, TResult>(feature);
        }

        public static Task RunCommandAsync<TDomain>(this IDomainInstance<TDomain> source,
            IAsyncCommandFeature<TDomain> feature) where TDomain : IDomain
        {
            return source.RunFeatureAsync<ICommandDomainRole<TDomain>>(feature);
        }
    }
}