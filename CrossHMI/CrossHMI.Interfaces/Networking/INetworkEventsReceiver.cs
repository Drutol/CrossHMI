using System.Threading.Tasks;

namespace CrossHMI.Interfaces.Networking
{
    public interface INetworkEventsReceiver
    {
        Task Initialize();

        INetworkVariableUpdateSource<T> ObtainEventSourceForVariable<T>(string repository, string variableName);

        INetworkDeviceUpdateSource<TDevice> ObtainEventSourceForDevice<TDevice>(string repository) 
            where TDevice : INetworkDevice , new ();
    }
}
