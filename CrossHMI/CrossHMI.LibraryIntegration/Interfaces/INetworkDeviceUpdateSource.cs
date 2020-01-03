namespace CrossHMI.Interfaces.Networking
{
    /// <summary>
    ///     Component that forwards updates from library to appropriate <see cref="INetworkDevice" />.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INetworkDeviceUpdateSource<out T>
        : INetworkDeviceUpdateSourceBase
        where T : INetworkDevice
    {
        /// <summary>
        ///     The device that is receiving the updates.
        /// </summary>
        T Device { get; }
    }
}