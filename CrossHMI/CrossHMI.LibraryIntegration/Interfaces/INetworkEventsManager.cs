using System;
using System.Threading.Tasks;

namespace CrossHMI.LibraryIntegration.Interfaces
{
    /// <summary>
    /// Root class that initializes the library and abstraction layer of itself.
    /// Allows to create <see cref="INetworkDevice" /> instances.
    /// </summary>
    public interface INetworkEventsManager
    {
        /// <summary>
        /// Called whenever the library reports new repository.
        /// </summary>
        event EventHandler<INetworkDeviceUpdateSource<INetworkDynamicDevice>> NewDeviceCreated;

        /// <summary>
        /// Initializes the underlying library.
        /// </summary>
        Task Initialize();

        /// <summary>
        /// Enables automatic creation of new devices if unknown repository manifests itself
        /// after for example providing new configuration to the library.
        /// </summary>
        /// <param name="deviceFactory">Factory used to instantiate device.</param>
        void EnableAutomaticDeviceInstantiation(Func<INetworkDynamicDevice> deviceFactory);

        /// <summary>
        /// Disables automatic creation of new devices.
        /// </summary>
        void DisableAutomaticDeviceInstantiation();

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
            Func<TDevice> factory)
            where TDevice : INetworkDevice;

        /// <summary>
        /// Obtains update source associated with given value within repository.
        /// </summary>
        /// <typeparam name="T">Type of the variable.</typeparam>
        /// <param name="repository">Repository.</param>
        /// <param name="variableName">Variable.</param>
        INetworkVariableUpdateSource<T> ObtainEventSourceForVariable<T>(string repository, string variableName);
    }
}