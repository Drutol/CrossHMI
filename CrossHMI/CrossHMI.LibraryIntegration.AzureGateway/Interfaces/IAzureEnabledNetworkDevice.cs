using Microsoft.Azure.Devices.Client;

namespace CrossHMI.LibraryIntegration.AzureGateway.Interfaces
{
    public interface IAzureEnabledNetworkDevice
    {
        /// <summary>
        /// Gets required connection parameters for establishing azure connection.
        /// </summary>
        IAzureDeviceParameters AzureDeviceParameters { get; }

        /// <summary>
        /// Gets or sets the device client. Will be updated upon successful registration.
        /// </summary>
        DeviceClient DeviceClient { get; set; }

        /// <summary>
        /// Returns JSON payload which will be passed to Azure.
        /// </summary>
        string CreateMessagePayload();
    }
}
