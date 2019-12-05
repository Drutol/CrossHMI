using CrossHMI.Interfaces.Networking;
using CrossHMI.Shared.Infrastructure;

namespace CrossHMI.Shared.Interfaces
{
    public interface INetworkDeviceDefinitionBuilderFactory
    {
        NetworkDeviceDefinitionBuilder<T> CreateBuilder<T>(INetworkDeviceUpdateSourceBase source) where T : INetworkDevice;
    }
}