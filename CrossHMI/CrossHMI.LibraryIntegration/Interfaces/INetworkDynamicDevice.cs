namespace CrossHMI.LibraryIntegration.Interfaces
{
    /// <summary>
    /// Interface extending the <see cref="INetworkDevice"/> with dynamic configuration capabilities.
    /// </summary>
    public interface INetworkDynamicDevice : INetworkDevice
    {
        /// <summary>
        /// The dynamic handle which will receive binding creation notifications and reconfigure parent <see cref="INetworkDeviceUpdateSource{T}"/>
        /// </summary>
        INetworkDeviceDynamicLifetimeHandle Handle { get; }
    }
}
 