using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;
using D3SK.NetCore.Domain.Events.EntityEvents;
using D3SK.NetCore.Domain.Models;
using D3SK.NetCore.Infrastructure.Domain;
using ExampleBookstore.Infrastructure;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Entities;
using ExampleBookstore.Services.BookService.Domain.Features.AuthorFeatures;
using ExampleBookstore.Services.BookService.Domain.Features.BookFeatures;
using ExampleBookstore.Services.BookService.Domain.Stores;
using ExampleBookstore.Services.BookService.Infrastructure.Events;
using ExampleBookstore.Services.BookService.Infrastructure.Features.AuthorFeatures;
using ExampleBookstore.Services.BookService.Infrastructure.Features.BookFeatures;
using ExampleBookstore.Services.BookService.Infrastructure.Stores;
using ExampleBookstore.Services.BookService.Infrastructure.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExampleBookstore.Services.BookService.Infrastructure
{
    public class BookDomain : ExampleBookstoreDomain<IBookDomain>, IBookDomain
    {
        public BookDomain(
            IDomainBus bus,
            IQueryDomainRole<IBookDomain> queryRole,
            ICommandDomainRole<IBookDomain> commandRole,
            IHandleDomainEventStrategy<IDomainEvent, IBookDomain> eventStrategy,
            IHandleValidationStrategy<IBookDomain> validationStrategy)
            : base(bus, queryRole, commandRole, eventStrategy, validationStrategy)
        {
            ConfigureDomain(this);
        }

        public static void ConfigureDomain(IBookDomain domain)
        {
            domain.HandlesBusEvent<EntityUpdatedBusEventHandler, EntityUpdatedBusEvent>();
            domain.HandlesValidation<BookValidator, Book, PatchEntityValidationOptions>();
            domain.HandlesValidation<BookCreateCommandValidator, BookCreateCommand>();
            domain.HandlesValidation<BookUpdateCommandValidator, BookUpdateCommand>();
        }

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // db
            var connectionString = configuration.GetConnectionString($"{nameof(BookDbStore)}ConnectionString");
            services.AddDbContext<BookDbStore>(options => 
                options.UseSqlServer(connectionString).EnableSensitiveDataLogging());

            // stores
            services.AddScoped<IBookStore>(provider => provider.GetService<BookDbStore>());
            services.AddScoped<IBookQueryStore>(provider => provider.GetService<BookDbStore>());

            // containers
            services.AddTransient<IAuthorCommandContainer, AuthorDbCommandContainer>();
            services.AddTransient<IAuthorQueryContainer, AuthorDbQueryContainer>();
            services.AddTransient<IBookCommandContainer, BookDbCommandContainer>();
            services.AddTransient<IBookQueryContainer, BookDbQueryContainer>();
            
            // domain
            services.AddSingleton<IBookDomain, BookDomain>();
            services.AddScoped<IDomainInstance<IBookDomain>, BookDomainInstance>();
            DomainBaseStartup.ConfigureDefaultEventStrategies<IBookDomain>(services, configuration);

            // roles
            services.AddTransient<IQueryDomainRole<IBookDomain>, ExampleBookstoreQueryDomainRole<IBookDomain>>();
            services.AddTransient<ICommandDomainRole<IBookDomain>, ExampleBookstoreCommandDomainRole<IBookDomain>>();

            // author features
            services.AddTransient<IAuthorCountQuery, AuthorCountQuery>();
            services.AddTransient<IAuthorQuery, AuthorQuery>();
            services.AddTransient<IAuthorProjectionQuery, AuthorProjectionQuery>();
            services.AddTransient<IAuthorCreateCommand, AuthorCreateCommand>();
            services.AddTransient<IAuthorUpdateCommand, AuthorUpdateCommand>();
            services.AddTransient<IAuthorDeleteCommand, AuthorDeleteCommand>();

            // book features
            services.AddTransient<IBookCountQuery, BookCountQuery>();
            services.AddTransient<IBookQuery, BookQuery>();
            services.AddTransient<IBookProjectionQuery, BookProjectionQuery>();
            services.AddTransient<IBookCreateCommand, BookCreateCommand>();
            services.AddTransient<IBookUpdateCommand, BookUpdateCommand>();
            services.AddTransient<IBookDeleteCommand, BookDeleteCommand>();
        }
    }
}
