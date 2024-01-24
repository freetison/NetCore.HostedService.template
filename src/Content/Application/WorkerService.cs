using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using nchsapp.Services;
using Polly;

namespace nchsapp
{
    public class WorkerService : IHostedService, IDisposable
    {
        private readonly ILogger<WorkerService> _logger;
        private readonly IGoogleService _googleService;

        public WorkerService(ILogger<WorkerService> logger, IGoogleService googleService)
        {
            _logger = logger;
            _googleService = googleService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service started !!!");
            await BackgroundProcessing(cancellationToken);
        }


        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
           await Policy
                .HandleResult<bool>(c => c == false)
                .WaitAndRetryForeverAsync(count => TimeSpan.FromSeconds(count))
                .ExecuteAsync(async () =>
                {
                    _logger.LogInformation($"Pinging {_googleService.IPAddress} {await _googleService.Ping()}");
                    return false;
                });

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service stoped !!!");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _googleService.Dispose();
        }
    }
}
