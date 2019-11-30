using System;
using System.Threading.Tasks;

namespace CrossHMI.Interfaces.Networking
{
    /// <summary>
    ///     Root class that initializes the library and abstraction layer of itself.
    ///     Allows to create <see cref="INetworkDevice" /> instances.
    /// </summary>
    public interface INetworkEventsManager
    {
        /// <summary>
        ///     Initializes the underlying library.
        /// </summary>
        Task Initialize();

        /// <summary>
        ///     Builds the instance of given type.
        ///     Subscribes the model for network updates as defined in the configuration for given repository.
        /// </summary>
        /// <typeparam name="TDevice">The type of the device.</typeparam>
        /// <param name="repository">The repository against which the device will be created.</param>
        /// <param name="factory">Device instance factory.</param>
        /// <returns>Device model.</returns>
        INetworkDeviceUpdateSource<TDevice> ObtainEventSourceForDevice<TDevice>(
            string repository,
            Func<TDevice> factory = null)
            where TDevice : INetworkDevice;
    }
}