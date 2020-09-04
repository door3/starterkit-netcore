using System.Threading.Tasks;
using D3SK.NetCore.Api;
using ExampleBookstore.Services.BookService.Infrastructure.Stores;
using Microsoft.Extensions.Hosting;

namespace ExampleBookstore.Services.ServicesGateway.Api
{
    public class Program
    {
        public static Task Main(string[] args)
            => CreateHostBuilder(args)
                .Build()
                .MigrateDbStores(typeof(BookDbStore))
                .RunAsync();

        public static IHostBuilder CreateHostBuilder(string[] args)
            => StartupHelper.CreateDefaultWebHostBuilder<Startup>(args);
    }
}
