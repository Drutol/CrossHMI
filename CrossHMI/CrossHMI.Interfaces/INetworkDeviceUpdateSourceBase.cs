using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CrossHMI.Interfaces
{
    public interface INetworkDeviceUpdateSourceBase
    {
        event PropertyChangedEventHandler Updated;

        void RegisterNetworkVariable<TProperty>(INetworkVariableUpdateSource<TProperty> networkVariableUpdateSource);
    }
}
