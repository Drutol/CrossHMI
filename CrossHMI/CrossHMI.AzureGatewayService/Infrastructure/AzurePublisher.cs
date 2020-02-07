using System;
using System.Threading;
using System.Threading.Tasks;
using CrossHMI.AzureGatewayService.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CrossHMI.AzureGatewayService.Infrastructure
{
    public class AzurePublisher : BackgroundService, IAzurePublisher
    {
        private readonly ILogger<AzurePublisher> _logger;

        public AzurePublisher(ILogger<AzurePublisher> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
