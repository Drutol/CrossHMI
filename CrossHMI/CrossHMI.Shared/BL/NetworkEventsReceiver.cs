using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;
using CrossHMI.Interfaces;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Shared.BL.Consumer;
using CrossHMI.Shared.Configuration;
using CrossHMI.Shared.EventSources;
using UAOOI.Configuration.Networking;
using UAOOI.Configuration.Networking.Serialization;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.SemanticData.DataRepository;
using UAOOI.Networking.SemanticData.MessageHandling;

namespace CrossHMI.Shared.BL
{
    public partial class NetworkEventsReceiver<TConfiguration> : DataManagementSetup, INetworkEventsReceiver 
        where TConfiguration : ConfigurationData, new()
    {
        private readonly IRecordingBindingFactory _recordingBindingFactory;
        private readonly INetworkConfigurationProvider<TConfiguration> _configurationProvider;
        private readonly IDispatcherAdapter _dispatcherAdapter;

        public NetworkEventsReceiver(
            IRecordingBindingFactory bindingFactory,
            IConfigurationFactory configurationFactory,
            INetworkConfigurationProvider<TConfiguration> configurationProvider,
            IMessageHandlerFactory messageHandlerFactory, 
            IEncodingFactory encodingFactory,
            IDispatcherAdapter dispatcherAdapter)
        {
            _recordingBindingFactory = bindingFactory;
            _configurationProvider = configurationProvider;
            _dispatcherAdapter = dispatcherAdapter;

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

        public INetworkVariableUpdateSource<T> ObtainEventSourceForVariable<T>(string repository, string variableName)
        {
            var repositoryBindings = _recordingBindingFactory.GetConsumerBindingsForRepository(repository);
            var valueMonitor = repositoryBindings.First(pair => pair.Key.Equals(variableName)).Value;
            return new NetworkVariableEventSource<T>(valueMonitor, variableName);
        }

        public INetworkDeviceUpdateSource<TDevice> ObtainEventSourceForDevice<TDevice>(string repository)
            where TDevice : INetworkDevice, new()
        {
            var source = new NetworkDeviceEventSource<TDevice>(_dispatcherAdapter);

            source.Device = new NetworkDeviceDefinitionBuilder<TDevice>(this, source, repository).Build();
                         
            return source;
        }       
    }
}
