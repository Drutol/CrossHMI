using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using AoLibs.Adapters.Core.Interfaces;
using CrossHMI.Interfaces;

namespace CrossHMI.Shared.EventSources
{
    public class NetworkDeviceEventSource<T> : INetworkDeviceUpdateSource<T> 
        where T : INetworkDevice, new()
    {
        private readonly IDispatcherAdapter _dispatcherAdapter;

        public NetworkDeviceEventSource(IDispatcherAdapter dispatcherAdapter)
        {
            _dispatcherAdapter = dispatcherAdapter;
        }

        public T Device { get; set; }

        public event PropertyChangedEventHandler Updated;

        public void RegisterNetworkVariable<TProperty>(
            INetworkVariableUpdateSource<TProperty> networkVariableUpdateSource)
        {
            networkVariableUpdateSource.Updated += (updateSource, value) =>
            {
                _dispatcherAdapter.Run(() =>
                {
                    Device?.ProcessPropertyUpdate(updateSource.Name, value);
                });
            };
        }
    }
}
