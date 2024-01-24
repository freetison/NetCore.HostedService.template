using System;
using Flurl.Http.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using nchsapp;
using nchsapp.Services;


IHostBuilder builder = Host.CreateDefaultBuilder(args);
builder
    .UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development")
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        var env = hostingContext.HostingEnvironment;
        config.AddEnvironmentVariables();
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddLogging();
        services.AddSingleton(sp => 
                    new FlurlClientCache().Add("GoogleClient", "8.8.8.8"));
        services.AddTransient(sp => ActivatorUtilities.CreateInstance<Pinger>(sp));
        services.AddTransient<IGoogleService, GoogleService>(sp => ActivatorUtilities.CreateInstance<GoogleService>(sp));
        services.AddHostedService<WorkerService>();
        
    });


IHost host = builder.Build();
host.Run();

