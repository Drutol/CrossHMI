using System;
using System.Collections.Generic;
using System.Text;

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
