using ExampleBookstore.Domain;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;
using D3SK.NetCore.Infrastructure.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using D3SK.NetCore.Domain.Models;

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
            IOptions<DomainOptions> domainOptions,
            IDomainBus bus,
            IQueryDomainRole<TDomain> queryRole,
            ICommandDomainRole<TDomain> commandRole,
            IHandleDomainEventStrategy<IDomainEvent, TDomain> eventStrategy,
            IHandleValidationStrategy<TDomain> validationStrategy)
        : base(domainOptions, bus, queryRole, commandRole, eventStrategy, validationStrategy)
        {
        }
    }
}
