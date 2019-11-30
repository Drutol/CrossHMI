using System;
using System.Collections.Generic;
using System.Text;

namespace CrossHMI.Interfaces.Networking
{
    public interface INetworkDynamicDevice : INetworkDevice
    {
        INetworkDeviceDynamicLifetimeHandle Handle { get; }
    }
}
 