using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Api.Filters;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain.Models;
using D3SK.NetCore.Infrastructure.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        public static void ConfigureBaseServices(IServiceCollection services, IConfiguration configuration)
        {
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
                    new ResponseCacheAttribute() {NoStore = true, Location = ResponseCacheLocation.None});
            });

            services.AddApiVersioning(options => { options.AssumeDefaultVersionWhenUnspecified = true; });

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

            // configure base domain services
            DomainBaseStartup.ConfigureServices(services, configuration);
        }

        public static void ConfigureBaseApi(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseCors(AllowAllCorsPolicy);
            app.UseAuthentication();
            app.UseMultitenancy<ResolvedTenant>();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
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
                var context = (DbContext) serviceProvider.GetService(storeType);
                await context.Database.MigrateAsync();
            }
        }

        public static IHost MigrateDbStores(this IHost source, params Type[] storeTypes)
        {
            MigrateDbStoresAsync(source, storeTypes).Wait();
            return source;
        }
    }
}