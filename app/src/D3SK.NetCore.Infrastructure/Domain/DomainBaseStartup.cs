﻿using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain.Events;
using D3SK.NetCore.Domain.Models;
using D3SK.NetCore.Infrastructure.Events;
using D3SK.NetCore.Infrastructure.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace D3SK.NetCore.Infrastructure.Domain
{
    public static class DomainBaseStartup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IClock, DomainClock>();
            services.AddScoped<IExceptionManager, ExceptionManager>();
            services.AddMultitenancy<ResolvedTenant, TenantResolver>();
            services.AddScoped<ITenantManager, TenantManager>();
            services.Configure<QueryOptions>(configuration.GetSection(nameof(QueryOptions)));

            services
                .AddTransient<IHandleDomainMiddlewareStrategy<IDomainEvent>,
                    HandleDomainMiddlewareStrategy<IDomainEvent>>();
            services
                .AddTransient<IHandleDomainMiddlewareStrategy<IValidationEvent>,
                    HandleDomainMiddlewareStrategy<IValidationEvent>>();
        }
    }
}