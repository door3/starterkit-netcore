using D3SK.NetCore.Common;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;
using D3SK.NetCore.Infrastructure.Events;
using D3SK.NetCore.Infrastructure.Stores;
using D3SK.NetCore.Infrastructure.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace D3SK.NetCore.Infrastructure.Domain
{
    public static class DomainBaseStartup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, bool useMultitenancy = true)
        {
            services.AddTransient<IClock, DomainClock>();
            services.AddScoped<IExceptionManager, ExceptionManager>();
            services.Configure<QueryOptions>(configuration.GetSection(nameof(QueryOptions)));

            services
                .AddTransient<IHandleBusEventStrategy<IBusEvent>,
                    HandleBusEventStrategy<IBusEvent>>();

            services.AddTransient<IUpdateStrategy, OptimisticUpdateStrategy>();

            if (useMultitenancy)
            {
                services.AddScoped<ITenantManager, TenantManager>();
            }
            else
            {
                services.AddScoped<ITenantManager>(provider => new TenantManager(null));
            }

            services.AddSingleton<IDomainBus, MonolithicDomainBus>();
        }

        public static void ConfigureDefaultEventStrategies<TDomain>(IServiceCollection services, IConfiguration configuration) 
            where TDomain : IDomain
        {
            services
                .AddTransient<IHandleDomainEventStrategy<IDomainEvent, TDomain>,
                    HandleDomainEventStrategy<IDomainEvent, TDomain>>();
            services
                .AddTransient<IHandleValidationStrategy<TDomain>,
                    HandleValidationStrategy<TDomain>>();
        }
    }
}
