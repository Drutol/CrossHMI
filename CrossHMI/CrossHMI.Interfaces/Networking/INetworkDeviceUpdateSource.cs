namespace CrossHMI.Interfaces.Networking
{
    public interface INetworkDeviceUpdateSource<out T> : INetworkDeviceUpdateSourceBase
        where T : INetworkDevice, new ()
    {
        T Device { get; }
    }
}
