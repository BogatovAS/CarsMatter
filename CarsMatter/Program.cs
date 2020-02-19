using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace CarsMatter
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webhost = CreateWebHostBuilder(args).Build();

            //await webhost.InitAsync();

            webhost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
