using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CrossHMI.Interfaces
{
    public interface INetworkDeviceUpdateSource<T> : INetworkDeviceUpdateSourceBase
        where T : INetworkDevice, new ()
    {
        T Device { get; }
    }
}
