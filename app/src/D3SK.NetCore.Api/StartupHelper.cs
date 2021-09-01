using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using D3SK.NetCore.Api.Filters;
using D3SK.NetCore.Api.Utilities;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain.Models;
using D3SK.NetCore.Infrastructure.Domain;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authorization;
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
using Serilog.Sinks.Email;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Destructurama;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authentication;

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

                ConfigureEmailSink(hostingContext.Configuration, loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration))
                    .Destructure.UsingAttributes()
                    .Enrich.FromLogContext()
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss}] {SourceContext} [{Level:u3}] - {Message:l} | {Properties}{NewLine}{Exception}");
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
            IServiceCollection services, IConfiguration configuration, bool useMultitenancy = true, Action<MvcOptions> controllersConfigFn = null)
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

                if (controllersConfigFn != null)
                {
                    controllersConfigFn(options);
                }
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
            bool useMultitenancy = true, bool useSwagger = true,
            Action<IApplicationBuilder> configureMiddlewareFn = null,
            Action<IEndpointRouteBuilder> useEndpointsFn = null,
            Action<IApplicationBuilder> beforeAuthenticationFn = null)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseCors(AllowAllCorsPolicy);
            app.UseSerilogRequestLogging();

            if (beforeAuthenticationFn != null)
            {
                beforeAuthenticationFn(app);
            }

            app.UseAuthentication();

            if (useMultitenancy)
            {
                app.UseMultitenancy<ResolvedTenant>();
            }

            app.UseRouting();
            if (configureMiddlewareFn != null)
            {
                configureMiddlewareFn(app);
            }

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", GetHealthCheckOptions()).WithMetadata(new AllowAnonymousAttribute());
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

        private static LoggerConfiguration ConfigureEmailSink(IConfiguration configuration, LoggerConfiguration loggerConfiguration)
        {
            var options = configuration.Get<SerilogConfigurationOptions>();

            if (string.IsNullOrWhiteSpace(options.ErrorReportingRecipientEmails))
            {
                return loggerConfiguration;
            }

            var connectionInfo = new EmailConnectionInfo
            {
                MailServer = options.Smtp.Server,
                NetworkCredentials = new NetworkCredential(options.Smtp.Username, options.Smtp.Password),
                Port = options.Smtp.Port,
                EnableSsl = false,
                FromEmail = options.SupportEmail,
                ToEmail = options.ErrorReportingRecipientEmails,
                EmailSubject = $"Error Occurred ({options.PortalAppUrl})"
            };
  
            if (options.Smtp.IgnoreCertificateErrors)
            {
                connectionInfo.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            }
            else
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
                {
                    if (sslPolicyErrors == SslPolicyErrors.None)
                    {
                        return true;
                    }

                    if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
                    {
                        if (chain != null && chain.ChainStatus != null)
                        {
                            foreach (var status in chain.ChainStatus)
                            {
                                if (certificate.Subject == certificate.Issuer && status.Status == X509ChainStatusFlags.UntrustedRoot)
                                {
                                    continue;
                                }

                                if (status.Status != X509ChainStatusFlags.NoError)
                                {
                                    return false;
                                }
                            }
                        }

                        return true;
                    }

                    return false;
                };
            }

            return loggerConfiguration.WriteTo.Logger(
                c => c.WriteTo.Email(connectionInfo, period: TimeSpan.FromSeconds(options.EmailErrorReportingPeriod <= 0 ? 60 : options.EmailErrorReportingPeriod),
                    outputTemplate: "[{Timestamp:HH:mm:ss}] {SourceContext} [{Level:u3}] - {Message:l} | {Properties}{NewLine}{Exception}"),
                LogEventLevel.Error);
        }
    }

    public class SerilogConfigurationOptions
    {
        public string PortalAppUrl { get; set; }

        public string SupportEmail { get; set; }

        public string ErrorReportingRecipientEmails { get; set; }

        public int EmailErrorReportingPeriod { get; set; }

        public SmtpConfig Smtp { get; set; }
    }

    public class SmtpConfig
    {
        public string Server { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int Port { get; set; } = 25;

        public bool IsSsl { get; set; }

        public bool IgnoreCertificateErrors { get; set; }

        public bool DisableDestinationLimit { get; set; }

        public List<string> AllowedDestinations { get; set; }
    }
}
