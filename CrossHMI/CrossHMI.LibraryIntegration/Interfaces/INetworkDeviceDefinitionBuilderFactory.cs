namespace CrossHMI.LibraryIntegration.Interfaces
{
    public interface INetworkDeviceDefinitionBuilderFactory
    {
        INetworkDeviceDefinitionBuilder CreateBuilder<T>(INetworkDeviceUpdateSourceBase source) where T : INetworkDevice;
    }
}