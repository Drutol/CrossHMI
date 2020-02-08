using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CrossHMI.AzureGatewayService.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CrossHMI.AzureGatewayService.Infrastructure.Azure
{
    public class AzurePublisher : BackgroundService, IAzurePublisher
    {
        private readonly ILogger<AzurePublisher> _logger;
        private readonly List<AzurePublisherDeviceHandle> _deviceHandles = new List<AzurePublisherDeviceHandle>();

        public AzurePublisher(ILogger<AzurePublisher> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"AzurePublisher running.");

                foreach (var deviceHandle in _deviceHandles)
                {
                    try
                    {
                        await deviceHandle.PublishSelf();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Failed to publish data for handle {deviceHandle}");
                    }
                }

                _logger.LogInformation($"AzurePublisher finished processing {_deviceHandles.Count} handles.");
                await Task.Delay(10_000, stoppingToken);
            }
        }

        public async void RegisterDeviceForPublishing(IAzureEnabledNetworkDevice azureEnabledNetworkDevice)
        {
            var handle = new AzurePublisherDeviceHandle(azureEnabledNetworkDevice);
            try
            {
                if (await handle.Initialize().ConfigureAwait(false))
                {
                    _deviceHandles.Add(handle);
                }
                else
                {
                    _logger.LogError($"Failed to register device {azureEnabledNetworkDevice?.AzureConnectionParameters?.AzureDeviceId}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to register device {azureEnabledNetworkDevice?.AzureConnectionParameters?.AzureDeviceId}");
            }
        }
    }
}
