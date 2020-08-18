using System;
using System.Threading;
using System.Threading.Tasks;

namespace CrossHMI.LibraryIntegration.AzureGateway.Interfaces
{
    /// <summary>
    /// Component which manages azure gateway. Allows adding and removing specific devices.
    /// </summary>
    public interface IAzurePublisher 
    {
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
    }
}
