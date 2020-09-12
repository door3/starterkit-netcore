using System.Threading.Tasks;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Infrastructure.Domain;
using ExampleBookstore.Domain;

namespace ExampleBookstore.Infrastructure
{
    public class ExampleBookstoreQueryDomainRole<TDomain> : QueryDomainRoleBase<TDomain>
        where TDomain : IExampleBookstoreDomain<TDomain>
    {
    }

    public class ExampleBookstoreCommandDomainRole<TDomain> : CommandDomainRoleBase<TDomain>
        where TDomain : IExampleBookstoreDomain<TDomain>
    {
        public override async Task HandleFeatureAsync(IDomainInstance<TDomain> domainInstance, IAsyncCommandFeature<TDomain> feature)
        {
            if (await domainInstance.ValidateAsync(feature))
            {
                await base.HandleFeatureAsync(domainInstance, feature);
                return;
            }

            if (!domainInstance.ExceptionManager.HasErrors)
            {
                domainInstance.ExceptionManager.AddErrorMessage($"Domain validation of {feature.GetType().Name} failed.");
            }

            domainInstance.ExceptionManager.Throw();
        }
    }
}