namespace CrossHMI.Interfaces.Networking
{
    /// <summary>
    /// Ccomponent that forwards updates from library to appropriate <see cref="INetworkDevice"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INetworkDeviceUpdateSource<out T> 
        : INetworkDeviceUpdateSourceBase
        where T : INetworkDevice, new ()
    {
        /// <summary>
        /// The device that is receinving the updates.
        /// </summary>
        T Device { get; }
    }
}
