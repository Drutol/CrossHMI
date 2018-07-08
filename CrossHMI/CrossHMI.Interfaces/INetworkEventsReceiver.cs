using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrossHMI.Interfaces
{
    public interface INetworkEventsReceiver
    {
        Task Initialize();

        INetworkVariableUpdateSource<T> ObtainEventSourceForVariable<T>(string repository, string variableName);

        INetworkDeviceUpdateSource<TDevice> ObtainEventSourceForDevice<TDevice>(string repository) 
            where TDevice : INetworkDevice , new ();
    }
}
