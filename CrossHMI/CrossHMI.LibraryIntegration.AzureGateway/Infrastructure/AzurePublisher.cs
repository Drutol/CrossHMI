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
        private readonly SemaphoreSlim _handlesSemaphore = new SemaphoreSlim(1);
        private Func<IAzureEnabledNetworkDevice, AzurePublisherDeviceHandle> _handleFactory;

        ///<inheritdoc/>
        public TimeSpan PublishInterval { get; set; } = TimeSpan.FromSeconds(10);

        public AzurePublisher(
            Func<IAzureEnabledNetworkDevice, AzurePublisherDeviceHandle> handleFactory,
            ILogger<AzurePublisher> logger = null)
        {
            _logger = logger;
            _handleFactory = handleFactory;
        }

        ///<inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _handlesSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
                try
                {
                    _logger?.LogDebug($"AzurePublisher running.");

                    foreach (var deviceHandle in _deviceHandles)
                    {
                        try
                        {
                            await deviceHandle.PublishSelf();
                        }
                        catch (Exception e)
                        {
                            _logger?.LogError(e, $"Failed to publish data for device {deviceHandle}");
                        }
                    }

                    _logger?.LogDebug($"AzurePublisher finished processing {_deviceHandles.Count} devices.");
                }
                finally
                {
                    _handlesSemaphore.Release();
                }
                await Task.Delay(PublishInterval, cancellationToken);
            }
        }

        ///<inheritdoc/>
        public async Task CancelDevicePublishingAsync(IAzureEnabledNetworkDevice device)
        {
            await _handlesSemaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                var existingHandle = _deviceHandles.FirstOrDefault(handle => handle.Device == device);

                if (existingHandle is null)
                    throw new ArgumentException(
                        $"Provided {nameof(device)} parameter has not been registered for publishing.");

                _deviceHandles.Remove(existingHandle);
                await existingHandle.DisposeAsync();
            }
            finally
            {
                _handlesSemaphore.Release();
            }
        }

        ///<inheritdoc/>
        public async Task<bool> RegisterDeviceForPublishingAsync(IAzureEnabledNetworkDevice azureEnabledNetworkDevice)
        {
            var handle = _handleFactory(azureEnabledNetworkDevice);
            await _handlesSemaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                if (await handle.Initialize().ConfigureAwait(false))
                {
                    _deviceHandles.Add(handle);
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
            finally
            {
                _handlesSemaphore.Release();
            }

            return true;
        }

        public async ValueTask DisposeAsync()
        {
            _handlesSemaphore?.Dispose();
        }
    }
}
