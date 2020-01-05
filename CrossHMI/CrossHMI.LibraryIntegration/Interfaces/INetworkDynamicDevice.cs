namespace CrossHMI.LibraryIntegration.Interfaces
{
    public interface INetworkDynamicDevice : INetworkDevice
    {
        INetworkDeviceDynamicLifetimeHandle Handle { get; }
    }
}
 