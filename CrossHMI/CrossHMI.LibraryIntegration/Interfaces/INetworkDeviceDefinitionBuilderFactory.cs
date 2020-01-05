using CrossHMI.LibraryIntegration.Infrastructure;

namespace CrossHMI.LibraryIntegration.Interfaces
{
    public interface INetworkDeviceDefinitionBuilderFactory
    {
        NetworkDeviceDefinitionBuilder<T> CreateBuilder<T>(INetworkDeviceUpdateSourceBase source) where T : INetworkDevice;
    }
}