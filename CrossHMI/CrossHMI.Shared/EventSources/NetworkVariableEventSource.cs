using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Networking;
using UAOOI.Networking.SemanticData.DataRepository;

namespace CrossHMI.Shared.EventSources
{
    /// <inheritdoc />
    public class NetworkVariableEventSource<T> : INetworkVariableUpdateSource<T>
    {
        private readonly ConsumerBindingMonitoredValue<T> _monitoredValue;
        private NetworkVariableUpdateEventHandler<T> _updated;
        private bool _listening;

        /// <summary>
        /// Creates new instance of <see cref="NetworkVariableEventSource{T}"/>
        /// </summary>
        /// <param name="consumerBinding">Wrapped consumer binding.</param>
        /// <param name="variableName">The name of the variable which binding is being warpped.</param>
        public NetworkVariableEventSource(IConsumerBinding consumerBinding, string variableName)
        {
            Name = variableName;

            _monitoredValue = consumerBinding as ConsumerBindingMonitoredValue<T>;
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public event NetworkVariableUpdateEventHandler<T> Updated
        {
            add
            {
                if (!_listening)
                {
                    _monitoredValue.PropertyChanged += MonitoredValueOnPropertyChanged;
                    _listening = true;
                }

                _updated += value;
            }
            remove
            {
                _updated -= value;

                if (_updated == null)
                {
                    _monitoredValue.PropertyChanged -= MonitoredValueOnPropertyChanged;
                    _listening = false;
                }
            }
        }

        private void MonitoredValueOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _updated?.Invoke(this, _monitoredValue.Value);
        }
    }
}
