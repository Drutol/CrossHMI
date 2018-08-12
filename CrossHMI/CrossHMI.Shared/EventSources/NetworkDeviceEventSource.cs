using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using AoLibs.Adapters.Core.Interfaces;
using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Networking;

namespace CrossHMI.Shared.EventSources
{
    /// <inheritdoc />
    public class NetworkDeviceEventSource<T> : INetworkDeviceUpdateSource<T> 
        where T : INetworkDevice, new()
    {
        private readonly IDispatcherAdapter _dispatcherAdapter;

        /// <summary>
        /// Creates new instance of <see cref="NetworkDeviceEventSource{T}"/>.
        /// </summary>
        /// <param name="dispatcherAdapter">UI thread dispatcher</param>
        public NetworkDeviceEventSource(IDispatcherAdapter dispatcherAdapter)
        {
            _dispatcherAdapter = dispatcherAdapter;
        }

        /// <inheritdoc />
        public T Device { get; set; }

        /// <inheritdoc />
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
