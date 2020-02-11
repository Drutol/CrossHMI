namespace CrossHMI.LibraryIntegration.AzureGateway.Interfaces
{
    public interface IAzureEnabledNetworkDevice
    {
        IAzureConnectionParameters AzureConnectionParameters { get; }

        string CreateMessagePayload();
    }
}
