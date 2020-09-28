using System;
using System.IO;
using System.Threading.Tasks;
using D3SK.NetCore.Api.Filters;
using D3SK.NetCore.Api.Utilities;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain.Models;
using D3SK.NetCore.Infrastructure.Domain;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;

namespace D3SK.NetCore.Api
{
    public static class StartupHelper
    {
        public static readonly string AllowAllCorsPolicy = "AllowAllCorsPolicy";

        public static IHostBuilder CreateDefaultWebHostBuilder<TStartup>(string[] args) where TStartup : class
        {
            return Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseDefaultSerilog()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<TStartup>());
        }

        public static IConfiguration BuildWebConfiguration(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        public static IHostBuilder UseDefaultSerilog(this IHostBuilder source)
        {
            return source.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                Serilog.Debugging.SelfLog.Enable(Console.Error);

                loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                    .MinimumLevel.Information()
                    .MinimumLevel.Override(nameof(Microsoft), LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .WriteTo.Console();
            });
        }

        public static void AddDatabaseHealthCheck<TContext>(IServiceCollection services)
            where TContext : DbContext
        {
            services
                .AddHealthChecks()
                .AddDbContextCheck<TContext>(name: "database");
        }

        public static void ConfigureBaseServices(
            IServiceCollection services, IConfiguration configuration, bool useMultitenancy = true)
        {
            services.AddHealthChecks();

            // cors
            services.AddCors(options =>
            {
                options.AddPolicy(AllowAllCorsPolicy,
                    builder =>
                        builder.SetIsOriginAllowed(_ => true)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
            });

            // controllers
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ApiResultFilter));
                options.Filters.Add(typeof(ApiExceptionFilter));
                options.Filters.Add(
                    new ResponseCacheAttribute() { NoStore = true, Location = ResponseCacheLocation.None });
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new NewtonsoftPrivateSetterContractResolver();
            });

            services.AddApiVersioning(options => { options.AssumeDefaultVersionWhenUnspecified = true; });

            // http context accessor
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // misc
            services.AddOptions();
            services.AddMemoryCache();
            services.AddHttpClient();

            // response gzip compression
            services.AddResponseCompression();
            services.Configure<GzipCompressionProviderOptions>(options =>
                options.Level = System.IO.Compression.CompressionLevel.Fastest);
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });

            // current user manager/claims
            services.AddScoped<IUserClaims, UserClaims>();
            services.AddScoped<ICurrentUserManager<IUserClaims>, HttpCurrentUserManager<IUserClaims>>();

            if (useMultitenancy)
            {
                services.AddScoped<ITenantUserClaims, TenantUserClaims>();
                services.AddScoped<ICurrentUserManager<ITenantUserClaims>, HttpCurrentUserManager<ITenantUserClaims>>();
                services.Configure<MultitenancyOptions>(options => { });
                services.AddMultitenancy<ResolvedTenant, ClaimsTenantResolver>();
            }

            // configure base domain services
            DomainBaseStartup.ConfigureServices(services, configuration, useMultitenancy);

            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
                options.SchemaFilter<SwaggerExcludeFilter>();
            });
        }

        public static void ConfigureBaseApi(
            IApplicationBuilder app, IWebHostEnvironment env,
            bool useMultitenancy = true, bool useSwagger = true)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseCors(AllowAllCorsPolicy);
            app.UseAuthentication();

            if (useMultitenancy)
            {
                app.UseMultitenancy<ResolvedTenant>();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", GetHealthCheckOptions());
            });

            if (useSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "latest");
                });
            }
        }

        public static async Task MigrateDbStoresAsync(IHost host, params Type[] storeTypes)
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
                foreach (var storeType in storeTypes)
                {
                    await MigrateAsync(storeType, serviceProvider);
                }
            }

            async Task MigrateAsync(Type storeType, IServiceProvider serviceProvider)
            {
                var context = (DbContext)serviceProvider.GetService(storeType);
                await context.Database.MigrateAsync();
            }
        }

        public static IHost MigrateDbStores(this IHost source, params Type[] storeTypes)
        {
            MigrateDbStoresAsync(source, storeTypes).Wait();
            return source;
        }

        private static HealthCheckOptions GetHealthCheckOptions()
        {
            return new HealthCheckOptions()
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            };
        }
    }
}
