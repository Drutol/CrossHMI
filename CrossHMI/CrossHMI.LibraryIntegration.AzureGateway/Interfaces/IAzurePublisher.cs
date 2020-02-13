using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Optional;

namespace CrossHMI.LibraryIntegration.AzureGateway.Interfaces
{
    /// <summary>
    /// Component which manages azure gateway. Allows adding and removing specific devices.
    /// </summary>
    public interface IAzurePublisher : IDisposable
    {
        /// <summary>
        /// Gets or sets the time between data publish to Azure.
        /// </summary>
        TimeSpan PublishInterval { get; set; }

        /// <summary>
        /// Cancels publishing of the device and severs the connection.
        /// </summary>
        /// <param name="device">Device to disconnect.</param>
        Task CancelDevicePublishingAsync(IAzureEnabledNetworkDevice device);

        /// <summary>
        /// Registers device for publishing. Data from this device will be published during next scheduled publishing.
        /// </summary>
        /// <param name="device">The device to register.</param>
        Task<bool> RegisterDeviceForPublishingAsync(IAzureEnabledNetworkDevice device);


        /// <summary>
        /// Begins operation of the publisher. It will be awaited till the <param name="cancellationToken"/> won't be cancelled.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task StartAsync(CancellationToken cancellationToken);
    }
}
