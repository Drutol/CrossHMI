using System;

namespace CrossHMI.Interfaces.Networking
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
