using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;

namespace CrossHMI.LibraryIntegration.AzureGateway.Interfaces
{
    /// <summary>
    /// Interface defining device parameters for establishing azure connection.
    /// </summary>
    public interface IAzureDeviceParameters
    {
        /// <summary>
        /// Gets the transport type used for this device.
        /// </summary>
        TransportType TransportType { get; }

        /// <summary>
        /// Gets the Id corresponding to Azure device id.
        /// </summary>
        string AzureDeviceId { get; }

        /// <summary>
        /// Gets the azure scope id in which given device resides.
        /// </summary>
        string AzureScopeId { get; }

        /// <summary>
        /// Creates security client which will be used for device provisioning.
        /// </summary>
        Task<SecurityProvider> GetSecurityProviderAsync();
    }
}
