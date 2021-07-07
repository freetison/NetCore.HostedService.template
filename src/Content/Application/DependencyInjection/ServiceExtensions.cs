using System;
using Microsoft.Extensions.DependencyInjection;
using ncwsapp.Services;
using RestSharp;

namespace ncwsapp.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddHttpService(this IServiceCollection services, Action<HttpServiceOptions> setupAction)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

            services.Configure(setupAction);
            services.AddSingleton<HttpServiceOptions, HttpServiceOptions>();
            services.AddTransient<IRestClient, RestClient>();
            services.AddTransient<IHttpService, HttpService>();
            return services;
        }

    }

}
