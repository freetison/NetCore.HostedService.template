using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ncwsapp
{
    class Program
    {
        private static string _env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        static async Task<int> Main(string[] args)
        {
            try
            {
                await CreateHostBuilder(args).Build().RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"There was a problem");
                return 1;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .UseEnvironment(_env)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{_env}.json", optional: false, reloadOnChange: true);
                config.AddEnvironmentVariables();
                if (args != null) { config.AddCommandLine(args); }

                config.Build();

            })
            .ConfigureServices((hostContext, services) => new Startup(hostContext.Configuration).ConfigureServices(services))
            .UseSerilog((ctx, cfg) =>
            {
                cfg.ReadFrom.Configuration(ctx.Configuration)
                    .Enrich.WithMachineName()
                    .Enrich.WithProperty("Application", ctx.Configuration.GetValue<string>("ApplicationSettings:App"))
                    .Enrich.WithProperty("Version", ctx.Configuration.GetValue<string>("ApplicationSettings:Version"))
                    .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName)
                    .Enrich.FromLogContext();

            });
    }
}
