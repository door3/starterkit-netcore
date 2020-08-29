﻿using ExampleBookstore.Domain;
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

    public abstract class ExampleBookstoreDomain<TDomain> : DomainBase<TDomain> where TDomain : IExampleBookstoreDomain
    {
        protected ExampleBookstoreDomain(
            IQueryDomainRole<TDomain> queryRole,
            ICommandDomainRole<TDomain> commandRole,
            IHandleDomainMiddlewareStrategy<IDomainEvent> eventStrategy,
            IHandleDomainMiddlewareStrategy<IValidationEvent> validationStrategy)
        : base(queryRole, commandRole, eventStrategy, validationStrategy)
        {
        }
    }
}
