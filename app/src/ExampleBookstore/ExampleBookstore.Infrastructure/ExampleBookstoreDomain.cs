using ExampleBookstore.Domain;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;
using D3SK.NetCore.Infrastructure.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ExampleBookstore.Infrastructure
{
    public static class ExampleBookstoreDomain
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
        }
    }

    public abstract class ExampleBookstoreDomain<TDomain> : DomainBase<TDomain>, IExampleBookstoreDomain<TDomain>
        where TDomain : IExampleBookstoreDomain<TDomain>
    {
        protected ExampleBookstoreDomain(
            IDomainBus bus,
            IQueryDomainRole<TDomain> queryRole,
            ICommandDomainRole<TDomain> commandRole,
            IHandleDomainEventStrategy<IDomainEvent, TDomain> eventStrategy,
            IHandleValidationStrategy<TDomain> validationStrategy)
        : base(bus, queryRole, commandRole, eventStrategy, validationStrategy)
        {
        }
    }
}
