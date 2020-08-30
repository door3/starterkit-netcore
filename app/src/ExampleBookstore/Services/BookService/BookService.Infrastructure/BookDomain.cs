﻿using BookService.Infrastructure.Features.AuthorFeatures;
using BookService.Infrastructure.Features.BookFeatures;
using BookService.Infrastructure.Stores;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Events;
using ExampleBookstore.Infrastructure;
using ExampleBookstore.Services.BookService.Domain;
using ExampleBookstore.Services.BookService.Domain.Features.AuthorFeatures;
using ExampleBookstore.Services.BookService.Domain.Features.BookFeatures;
using ExampleBookstore.Services.BookService.Domain.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookService.Infrastructure
{
    public class BookDomain : ExampleBookstoreDomain<IBookDomain>, IBookDomain
    {
        public BookDomain(
            IQueryDomainRole<IBookDomain> queryRole,
            ICommandDomainRole<IBookDomain> commandRole,
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
            // db
            var connectionString = configuration.GetConnectionString("BookDbStoreConnectionString");
            services.AddDbContext<BookDbStore>(options => 
                options.UseSqlServer(connectionString).EnableSensitiveDataLogging());

            // stores
            services.AddScoped<IBookStore>(provider => provider.GetService<BookDbStore>());
            services.AddScoped<IBookQueryStore>(provider => provider.GetService<BookDbStore>());

            // containers
            services.AddScoped<IAuthorCommandContainer, AuthorDbCommandContainer>();
            services.AddScoped<IAuthorQueryContainer, AuthorDbQueryContainer>();
            services.AddScoped<IBookCommandContainer, BookDbCommandContainer>();
            services.AddScoped<IBookQueryContainer, BookDbQueryContainer>();
            
            // domain
            services.AddSingleton<IBookDomain, BookDomain>();
            services.AddScoped<IDomainInstance<IBookDomain>, BookDomainInstance>();

            // author features
            services.AddScoped<IAuthorCountQuery, AuthorCountQuery>();
            services.AddScoped<IAuthorQuery, AuthorQuery>();
            services.AddScoped<IAuthorProjectionQuery, AuthorProjectionQuery>();
            services.AddScoped<IAuthorCreateCommand, AuthorCreateCommand>();
            services.AddScoped<IAuthorUpdateCommand, AuthorUpdateCommand>();
            services.AddScoped<IAuthorDeleteCommand, AuthorDeleteCommand>();

            // book features
            services.AddScoped<IBookCountQuery, BookCountQuery>();
            services.AddScoped<IBookQuery, BookQuery>();
            services.AddScoped<IBookProjectionQuery, BookProjectionQuery>();
            services.AddScoped<IBookCreateCommand, BookCreateCommand>();
            services.AddScoped<IBookUpdateCommand, BookUpdateCommand>();
            services.AddScoped<IBookDeleteCommand, BookDeleteCommand>();

            // roles
            services.AddTransient<IQueryDomainRole<IBookDomain>, ExampleBookstoreQueryDomainRole<IBookDomain>>();
            services.AddTransient<ICommandDomainRole<IBookDomain>, ExampleBookstoreCommandDomainRole<IBookDomain>>();
        }
    }
}
