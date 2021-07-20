using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TribeTussleGameDataEditor
{
    public class Program
    {
        public const string AppName = "TribeTussleGameDataEditor";
        public const string AppDisplayName = "Tribe Tussle Game Data Editor";

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
