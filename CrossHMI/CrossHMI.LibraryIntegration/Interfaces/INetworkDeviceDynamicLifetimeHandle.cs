using System;

namespace CrossHMI.LibraryIntegration.Interfaces
{
    public interface INetworkDeviceDynamicLifetimeHandle
    {
        INetworkDeviceUpdateSourceBase DeviceUpdateSourceBase { get; set; }

        void NotifyNewBindingCreated(
            string repository,
            string processValue, 
            Type bindingType);
    }
}
