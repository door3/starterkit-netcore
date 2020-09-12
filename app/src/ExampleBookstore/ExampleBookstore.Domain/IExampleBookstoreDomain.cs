using D3SK.NetCore.Domain;

namespace ExampleBookstore.Domain
{
    public interface IExampleBookstoreDomain<TDomain> : IDomain<TDomain> where TDomain : IDomain
    {
    }
}
