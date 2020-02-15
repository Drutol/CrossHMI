using System;

namespace CrossHMI.LibraryIntegration.Interfaces
{
    /// <summary>
    /// This component is enabled to receive notifications of newly created bindings in case of configuration change.
    /// </summary>
    public interface INetworkDeviceDynamicLifetimeHandle
    {
        INetworkDeviceUpdateSourceBase DeviceUpdateSourceBase { get; set; }

        /// <summary>
        /// Called when the library requests new binding.
        /// </summary>
        /// <param name="repository">Repository of the device.</param>
        /// <param name="processValue">The name of the bound variable.</param>
        /// <param name="bindingType">Managed type of given variable.</param>
        void NotifyNewBindingCreated(
            string repository,
            string processValue, 
            Type bindingType);
    }
}
