using D3SK.NetCore.Api;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace ExampleBookstore.Services.BookService.Api
{
    public class Program
    {
        public static Task Main(string[] args)
            => CreateHostBuilder(args).Build().RunAsync();

        public static IHostBuilder CreateHostBuilder(string[] args)
            => StartupHelper.CreateDefaultWebHostBuilder<Startup>(args);
    }
}
