using Microsoft.Azure.Devices.Client;

namespace CrossHMI.LibraryIntegration.AzureGateway.Interfaces
{
    public interface IAzureConnectionParameters
    {
        TransportType TransportType { get; }

        string AzureDeviceId { get; }
        string AzureScopeId { get; }
        string AzurePrimaryKey { get; }
        string AzureSecondaryKey { get; }
    }
}
