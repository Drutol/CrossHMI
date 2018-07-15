using System.ComponentModel;

namespace CrossHMI.Interfaces.Networking
{
    public interface INetworkDeviceUpdateSourceBase
    {
        event PropertyChangedEventHandler Updated;

        void RegisterNetworkVariable<TProperty>(INetworkVariableUpdateSource<TProperty> networkVariableUpdateSource);
    }
}
