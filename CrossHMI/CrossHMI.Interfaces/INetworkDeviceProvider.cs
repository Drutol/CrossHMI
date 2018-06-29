using System;
using System.Collections.Generic;
using System.Text;

namespace CrossHMI.Interfaces
{
    public interface INetworkDeviceProvider
    {
        INetworkDeviceEventSource<T> RegisterDevice<T>();
    }
}
