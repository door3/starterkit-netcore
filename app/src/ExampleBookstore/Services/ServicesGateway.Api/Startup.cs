using BookService.Infrastructure;
using D3SK.NetCore.Api;
using D3SK.NetCore.Api.Utilities;
using D3SK.NetCore.Common.Utilities;
using ExampleBookstore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExampleBookstore.Services.ServicesGateway.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            Configuration = StartupHelper.BuildWebConfiguration(env);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            StartupHelper.ConfigureBaseServices(services, Configuration);
            ExampleBookstoreDomain.ConfigureServices(services, Configuration);
            BookDomain.ConfigureServices(services, Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StartupHelper.ConfigureBaseApi(app, env);
        }
    }
}
