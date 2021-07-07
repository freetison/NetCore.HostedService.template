using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ncwsapp.Services;
using Polly;
using Serilog;

namespace ncwsapp
{
    public class HostedServiceApp : IHostedService
    {
        private readonly IHttpService _httpService;

        public HostedServiceApp(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information("App started !!!");
            await BackgroundProcessing(cancellationToken);
        }


        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            var address = "8.8.8.8";


           await Policy
                .HandleResult<bool>(c => c == false)
                .WaitAndRetryForeverAsync(count => TimeSpan.FromSeconds(count))
                .ExecuteAsync(async () =>
                {
                    Log.Information($"Pinging {address} {await _httpService.Ping(IPAddress.Parse(address))}");
                    return false;
                });

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information("App stoped !!!");
            return Task.CompletedTask;
        }

    }
}
