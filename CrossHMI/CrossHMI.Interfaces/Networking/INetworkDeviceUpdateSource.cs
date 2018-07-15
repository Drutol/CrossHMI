namespace CrossHMI.Interfaces.Networking
{
    public interface INetworkDeviceUpdateSource<T> : INetworkDeviceUpdateSourceBase
        where T : INetworkDevice, new ()
    {
        T Device { get; }
    }
}
