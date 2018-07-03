using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossHMI.Interfaces;
using CrossHMI.Shared.BL.Consumer;
using UAOOI.Configuration.Networking;
using UAOOI.Configuration.Networking.Serialization;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.SemanticData.DataRepository;
using UAOOI.Networking.SemanticData.MessageHandling;

namespace CrossHMI.Shared.BL
{
    public class NetworkEventsReceiver : DataManagementSetup, INetworkEventsReceiver
    {
        private readonly IRecordingBindingFactory _recordingBindingFactory;

        public NetworkEventsReceiver(IRecordingBindingFactory bindingFactory, IConfigurationFactory configurationFactory,
            IMessageHandlerFactory messageHandlerFactory, IEncodingFactory encodingFactory)
        {
            _recordingBindingFactory = bindingFactory;

            BindingFactory = bindingFactory;
            ConfigurationFactory = configurationFactory;
            MessageHandlerFactory = messageHandlerFactory;
            EncodingFactory = encodingFactory;
        }

        public async Task Initialize()
        {
            await Task.Run(() =>
            {
                Start();
            });         
        }

        public INetworkVariableUpdateSource<T> ObtainEventSourceForVariable<T,TRaw>(string variableName) where T : INetworkVariable, new()
        {
            var valueMonitor = _recordingBindingFactory.ConsumerBindings.First(pair => pair.Key.Equals(variableName)).Value;
            return new NetworkVariableEventSource<T,TRaw>(valueMonitor, variableName);
        }

        class NetworkVariableEventSource<T,TRaw> : INetworkVariableUpdateSource<T> where T : INetworkVariable, new()
        {
            private readonly string _variableName;
            private readonly ConsumerBindingMonitoredValue<TRaw> _monitoredValue;
            private EventHandler<T> _updated;
            private bool _listening;

            public NetworkVariableEventSource(IConsumerBinding consumerBinding, string variableName)
            {
                _variableName = variableName;
                _monitoredValue = consumerBinding as ConsumerBindingMonitoredValue<TRaw>;
            }
           
            public event EventHandler<T> Updated
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
                var variable = new T {Name = _variableName};
                variable.Initialize(sender.ToString());
                _updated?.Invoke(this,variable);
            }

            public void Dispose()
            {
                _monitoredValue.PropertyChanged -= MonitoredValueOnPropertyChanged;
                _listening = false;
                _updated = null;
            }
        }
    }
}
