using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;
using ExampleBookstore.Domain;
using ExampleBookstore.Infrastructure;
using ExampleBookstore.Services.BookService.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookService.Infrastructure
{
    public class BookDomain : ExampleBookstoreDomain<BookDomain>, IBookDomain
    {
        public BookDomain(
            IQueryDomainRole<BookDomain> queryRole,
            ICommandDomainRole<BookDomain> commandRole,
            IHandleDomainMiddlewareStrategy<IDomainEvent> eventStrategy,
            IHandleDomainMiddlewareStrategy<IValidationEvent> validationStrategy)
            : base(queryRole, commandRole, eventStrategy, validationStrategy)
        {
            ConfigureDomain(this);
        }

        public static void ConfigureDomain(IBookDomain domain)
        {
        }

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // domain
            services.AddSingleton<IBookDomain, BookDomain>();
            services.AddScoped<IDomainInstance<IBookDomain>, BookDomainInstance>();

            // roles
            services.AddTransient<IQueryDomainRole<IBookDomain>, ExampleBookstoreQueryDomainRole<IBookDomain>>();
            services.AddTransient<ICommandDomainRole<IBookDomain>, ExampleBookstoreCommandDomainRole<IBookDomain>>();
        }
    }
}
