using System.ComponentModel;
using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Networking;
using UAOOI.Networking.SemanticData.DataRepository;

namespace CrossHMI.Shared.EventSources
{
    /// <inheritdoc />
    public class NetworkVariableEventSource<T> : INetworkVariableUpdateSource<T>
    {
        private readonly ConsumerBindingMonitoredValue<T> _monitoredValue;

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public event NetworkVariableUpdateEventHandler<T> Updated;

        /// <summary>
        ///     Creates new instance of <see cref="NetworkVariableEventSource{T}" />
        /// </summary>
        /// <param name="consumerBinding">Wrapped consumer binding.</param>
        /// <param name="variableName">The name of the variable which binding is being warpped.</param>
        public NetworkVariableEventSource(IConsumerBinding consumerBinding, string variableName)
        {
            Name = variableName;

            _monitoredValue = (ConsumerBindingMonitoredValue<T>) consumerBinding;
            _monitoredValue.PropertyChanged += MonitoredValueOnPropertyChanged;
        }

        private void MonitoredValueOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Updated?.Invoke(this, _monitoredValue.Value);
        }
    }
}