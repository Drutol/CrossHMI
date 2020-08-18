using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrossHMI.LibraryIntegration.AzureGateway.Interfaces;
using Microsoft.Extensions.Logging;

namespace CrossHMI.LibraryIntegration.AzureGateway.Infrastructure
{
    ///<inheritdoc/>
    public class AzurePublisher : IAzurePublisher
    {
        private readonly ILogger<AzurePublisher> _logger;
        private readonly List<AzurePublisherDeviceHandle> _deviceHandles = new List<AzurePublisherDeviceHandle>();
        private Func<IAzureEnabledNetworkDevice, AzurePublisherDeviceHandle> _handleFactory;

        public AzurePublisher(
            Func<IAzureEnabledNetworkDevice, AzurePublisherDeviceHandle> handleFactory,
            ILogger<AzurePublisher> logger = null)
        {
            _logger = logger;
            _handleFactory = handleFactory;
        }

        ///<inheritdoc/>
        public async Task CancelDevicePublishingAsync(IAzureEnabledNetworkDevice device)
        {
            var existingHandle = _deviceHandles.FirstOrDefault(handle => handle.Device == device);

            if (existingHandle is null)
                throw new ArgumentException(
                    $"Provided {nameof(device)} parameter has not been registered for publishing.");

            _deviceHandles.Remove(existingHandle);
            await existingHandle.DisposeAsync();
        }

        ///<inheritdoc/>
        public async Task<bool> RegisterDeviceForPublishingAsync(IAzureEnabledNetworkDevice azureEnabledNetworkDevice)
        {
            try
            {
                var handle = _handleFactory(azureEnabledNetworkDevice);
                if (await handle.Initialize().ConfigureAwait(false))
                {
                    handle.StartPublishing();
                }
                else
                {
                    _logger?.LogError(
                        $"Failed to register device {azureEnabledNetworkDevice?.AzureDeviceParameters?.AzureDeviceId}");
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger?.LogError(e,
                    $"Failed to register device {azureEnabledNetworkDevice?.AzureDeviceParameters?.AzureDeviceId}");
                throw;
            }

            return true;
        }
    }
}
