using ExampleBookstore.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Entities;
using D3SK.NetCore.Domain.Events;
using D3SK.NetCore.Infrastructure.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ExampleBookstore.Infrastructure
{
    public class ExampleBookstoreDomain
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // domain
            
            // roles
        }
    }

    public abstract class ExampleBookstoreDomain<TDomain> : DomainBase where TDomain : IExampleBookstoreDomain
    {
        protected ExampleBookstoreDomain(
            IQueryDomainRole<TDomain> queryRole,
            ICommandDomainRole<TDomain> commandRole,
            IHandleDomainMiddlewareStrategy<IDomainEvent> eventStrategy,
            IHandleDomainMiddlewareStrategy<IValidationEvent> validationStrategy)
        : base(eventStrategy, validationStrategy)
        {
            AddRole(queryRole.NotNull(nameof(queryRole)));
            AddRole(commandRole.NotNull(nameof(commandRole)));
        }
    }
}
