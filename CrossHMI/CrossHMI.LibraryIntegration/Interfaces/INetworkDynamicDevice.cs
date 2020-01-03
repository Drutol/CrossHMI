namespace CrossHMI.Interfaces.Networking
{
    public interface INetworkDynamicDevice : INetworkDevice
    {
        INetworkDeviceDynamicLifetimeHandle Handle { get; }
    }
}
 