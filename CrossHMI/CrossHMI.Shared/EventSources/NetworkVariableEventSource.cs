using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Networking;
using UAOOI.Networking.SemanticData.DataRepository;

namespace CrossHMI.Shared.EventSources
{
    public class NetworkVariableEventSource<T> : INetworkVariableUpdateSource<T>
    {
        private readonly ConsumerBindingMonitoredValue<T> _monitoredValue;
        private NetworkVariableUpdateEventHandler<T> _updated;
        private bool _listening;

        public NetworkVariableEventSource(IConsumerBinding consumerBinding, string variableName)
        {
            Name = variableName;

            _monitoredValue = consumerBinding as ConsumerBindingMonitoredValue<T>;
        }

        public string Name { get; }

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

        public void Dispose()
        {
            _monitoredValue.PropertyChanged -= MonitoredValueOnPropertyChanged;
            _listening = false;
            _updated = null;
        }
    }
}
