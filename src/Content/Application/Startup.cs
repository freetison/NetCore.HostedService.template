using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using nchsapp.DependencyInjection;
using nchsapp.Models;
using nchsapp.Services;

namespace nchsapp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            // App Settings
            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));

            // Add options
            services.AddOptions();

            // Add hhtClient
            services.AddHttpService(options =>
            {
                options.BaseUrl = "https://google.com";
                //options.Authenticator = new HttpBasicAuthenticator("user", "xxxxxxxx");
            });


            // Add app services
            services.AddHostedService<HostedServiceApp>();
            services.AddTransient(sp => ActivatorUtilities.CreateInstance<Pinger>(sp));

            return services;
        }

    }
}